using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetPromotions;

public class GetPromotionsQueryHandler : IRequestHandler<GetPromotionsQuery, List<PromotionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPromotionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PromotionDto>> Handle(GetPromotionsQuery request, CancellationToken cancellationToken)
    {
        var promotions = await _unitOfWork.Promotions.GetAllAsync();

        // Apply filters
        var query = promotions.AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(p => p.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(p => p.StartDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(p => p.EndDate <= request.ToDate.Value);
        }

        var result = query.OrderByDescending(p => p.CreatedAt).ToList();

        return _mapper.Map<List<PromotionDto>>(result);
    }
}


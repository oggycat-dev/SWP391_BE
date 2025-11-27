using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetActivePromotions;

public class GetActivePromotionsQueryHandler : IRequestHandler<GetActivePromotionsQuery, List<PromotionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActivePromotionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PromotionDto>> Handle(GetActivePromotionsQuery request, CancellationToken cancellationToken)
    {
        var promotions = await _unitOfWork.Promotions.GetActivePromotionsAsync();
        
        return _mapper.Map<List<PromotionDto>>(promotions);
    }
}


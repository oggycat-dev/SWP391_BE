using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Queries.GetDealers;

public class GetDealersQueryHandler : IRequestHandler<GetDealersQuery, PaginatedResult<DealerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<DealerDto>> Handle(GetDealersQuery request, CancellationToken cancellationToken)
    {
        var dealers = await _unitOfWork.Dealers.GetAllAsync();
        var filtered = dealers.AsEnumerable();

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<DealerStatus>(request.Status, out var status))
        {
            filtered = filtered.Where(d => d.Status == status);
        }

        if (!string.IsNullOrEmpty(request.City))
        {
            filtered = filtered.Where(d => d.City.Contains(request.City, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filtered = filtered.Where(d =>
                d.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.DealerCode.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var total = filtered.Count();
        var items = filtered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<DealerDto>>(items);

        return new PaginatedResult<DealerDto>
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

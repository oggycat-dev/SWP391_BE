using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariants;

public class GetVehicleVariantsQueryHandler : IRequestHandler<GetVehicleVariantsQuery, PaginatedResult<VehicleVariantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleVariantsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<VehicleVariantDto>> Handle(GetVehicleVariantsQuery request, CancellationToken cancellationToken)
    {
        var variants = await _unitOfWork.VehicleVariants.GetAllWithModelsAsync();

        var filtered = variants.AsEnumerable();

        // Apply filters
        if (request.ModelId.HasValue)
        {
            filtered = filtered.Where(v => v.ModelId == request.ModelId.Value);
        }

        if (request.IsActive.HasValue)
        {
            filtered = filtered.Where(v => v.IsActive == request.IsActive.Value);
        }

        // Apply search
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filtered = filtered.Where(v =>
                v.VariantName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.VariantCode.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var total = filtered.Count();
        var items = filtered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<VehicleVariantDto>>(items);

        return new PaginatedResult<VehicleVariantDto>
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

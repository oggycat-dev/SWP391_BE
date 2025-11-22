using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventories;

public class GetVehicleInventoriesQueryHandler : IRequestHandler<GetVehicleInventoriesQuery, PaginatedResult<VehicleInventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleInventoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<VehicleInventoryDto>> Handle(GetVehicleInventoriesQuery request, CancellationToken cancellationToken)
    {
        var inventories = await _unitOfWork.VehicleInventories.GetAllWithDetailsAsync();

        var filtered = inventories.AsEnumerable();

        if (request.VariantId.HasValue)
        {
            filtered = filtered.Where(i => i.VariantId == request.VariantId.Value);
        }

        if (request.ColorId.HasValue)
        {
            filtered = filtered.Where(i => i.ColorId == request.ColorId.Value);
        }

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<VehicleStatus>(request.Status, out var status))
        {
            filtered = filtered.Where(i => i.Status == status);
        }

        if (!string.IsNullOrEmpty(request.WarehouseLocation) && Enum.TryParse<WarehouseLocation>(request.WarehouseLocation, out var location))
        {
            filtered = filtered.Where(i => i.WarehouseLocation == location);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filtered = filtered.Where(i =>
                i.VINNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                i.ChassisNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                i.EngineNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var total = filtered.Count();
        var items = filtered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<VehicleInventoryDto>>(items);

        return new PaginatedResult<VehicleInventoryDto>
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

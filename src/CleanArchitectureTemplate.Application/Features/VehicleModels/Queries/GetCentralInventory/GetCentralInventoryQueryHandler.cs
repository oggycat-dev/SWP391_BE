using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetCentralInventory;

public class GetCentralInventoryQueryHandler : IRequestHandler<GetCentralInventoryQuery, List<VehicleInventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCentralInventoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<VehicleInventoryDto>> Handle(GetCentralInventoryQuery request, CancellationToken cancellationToken)
    {
        var inventories = await _unitOfWork.VehicleInventories.GetAllWithDetailsAsync();

        // Filter for central warehouse (vehicles not allocated to dealers)
        var centralInventories = inventories
            .Where(i => i.WarehouseLocation == WarehouseLocation.Central && !i.DealerId.HasValue)
            .ToList();

        if (request.VehicleVariantId.HasValue)
        {
            centralInventories = centralInventories
                .Where(i => i.VariantId == request.VehicleVariantId.Value)
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            if (Enum.TryParse<VehicleStatus>(request.Status, out var status))
            {
                centralInventories = centralInventories
                    .Where(i => i.Status == status)
                    .ToList();
            }
        }

        return _mapper.Map<List<VehicleInventoryDto>>(centralInventories);
    }
}


using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetCentralInventory;

public class GetCentralInventoryQuery : IRequest<List<VehicleInventoryDto>>
{
    public Guid? VehicleVariantId { get; set; }
    public string? Status { get; set; }
}


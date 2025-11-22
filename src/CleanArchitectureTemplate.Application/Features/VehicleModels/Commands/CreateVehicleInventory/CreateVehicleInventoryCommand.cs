using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleInventory;

public class CreateVehicleInventoryCommand : IRequest<VehicleInventoryDto>
{
    public Guid VariantId { get; set; }
    public Guid ColorId { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string ChassisNumber { get; set; } = string.Empty;
    public string EngineNumber { get; set; } = string.Empty;
    public DateTime ManufactureDate { get; set; }
    public DateTime ImportDate { get; set; }
    public string WarehouseLocation { get; set; } = string.Empty;
}

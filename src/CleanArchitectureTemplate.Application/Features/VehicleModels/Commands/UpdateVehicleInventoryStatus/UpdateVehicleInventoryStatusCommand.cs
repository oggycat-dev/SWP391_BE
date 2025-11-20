using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleInventoryStatus;

public class UpdateVehicleInventoryStatusCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? WarehouseLocation { get; set; }
}

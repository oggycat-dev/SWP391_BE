using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicle;

public class AllocateVehicleCommand : IRequest<Unit>
{
    public Guid InventoryId { get; set; }
    public Guid OrderId { get; set; }
}

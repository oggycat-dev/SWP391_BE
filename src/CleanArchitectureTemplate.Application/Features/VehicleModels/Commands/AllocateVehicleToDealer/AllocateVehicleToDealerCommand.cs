using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicleToDealer;

public class AllocateVehicleToDealerCommand : IRequest<Unit>
{
    public Guid InventoryId { get; set; }
    public Guid DealerId { get; set; }
}


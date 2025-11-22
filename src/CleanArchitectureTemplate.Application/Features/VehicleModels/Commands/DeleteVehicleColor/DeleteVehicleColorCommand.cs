using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleColor;

public class DeleteVehicleColorCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

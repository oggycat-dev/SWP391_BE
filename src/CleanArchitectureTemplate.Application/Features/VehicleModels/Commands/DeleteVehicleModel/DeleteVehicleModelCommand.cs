using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleModel;

public class DeleteVehicleModelCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleVariant;

public class DeleteVehicleVariantCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

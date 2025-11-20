using MediatR;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleModel;

public class DeleteVehicleModelCommandHandler : IRequestHandler<DeleteVehicleModelCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleModelCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var vehicleModel = await _unitOfWork.VehicleModels.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle model with ID {request.Id} not found");

        await _unitOfWork.VehicleModels.DeleteAsync(vehicleModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

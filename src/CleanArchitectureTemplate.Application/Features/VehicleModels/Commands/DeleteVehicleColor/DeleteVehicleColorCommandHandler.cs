using MediatR;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleColor;

public class DeleteVehicleColorCommandHandler : IRequestHandler<DeleteVehicleColorCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleColorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteVehicleColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle color with ID {request.Id} not found");

        await _unitOfWork.VehicleColors.DeleteAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

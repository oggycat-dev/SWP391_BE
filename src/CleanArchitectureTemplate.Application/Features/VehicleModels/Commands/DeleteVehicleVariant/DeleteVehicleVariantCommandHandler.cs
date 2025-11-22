using MediatR;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleVariant;

public class DeleteVehicleVariantCommandHandler : IRequestHandler<DeleteVehicleVariantCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleVariantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteVehicleVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle variant with ID {request.Id} not found");

        await _unitOfWork.VehicleVariants.DeleteAsync(variant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

using MediatR;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicle;

public class AllocateVehicleCommandHandler : IRequestHandler<AllocateVehicleCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AllocateVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AllocateVehicleCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _unitOfWork.VehicleInventories.GetByIdAsync(request.InventoryId)
            ?? throw new NotFoundException($"Vehicle inventory with ID {request.InventoryId} not found");

        // Business rule: Only available vehicles can be allocated
        if (inventory.Status != VehicleStatus.Available)
        {
            throw new ValidationException($"Vehicle is not available for allocation. Current status: {inventory.Status}");
        }

        // Update inventory status
        inventory.Status = VehicleStatus.Reserved;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

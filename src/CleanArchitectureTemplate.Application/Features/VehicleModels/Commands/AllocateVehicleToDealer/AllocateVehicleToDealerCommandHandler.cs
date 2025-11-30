using MediatR;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicleToDealer;

public class AllocateVehicleToDealerCommandHandler : IRequestHandler<AllocateVehicleToDealerCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AllocateVehicleToDealerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AllocateVehicleToDealerCommand request, CancellationToken cancellationToken)
    {
        // Get inventory
        var inventory = await _unitOfWork.VehicleInventories.GetByIdWithDetailsAsync(request.InventoryId)
            ?? throw new NotFoundException($"Vehicle inventory with ID {request.InventoryId} not found");

        // Get dealer
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId)
            ?? throw new NotFoundException($"Dealer with ID {request.DealerId} not found");

        // Business rule: Only available vehicles can be allocated
        if (inventory.Status != VehicleStatus.Available)
        {
            throw new ValidationException($"Vehicle is not available for allocation. Current status: {inventory.Status}");
        }

        // Business rule: Vehicle must be in Central warehouse (not already allocated to another dealer)
        if (inventory.DealerId.HasValue)
        {
            throw new ValidationException($"Vehicle is already allocated to dealer ID: {inventory.DealerId}");
        }

        // Allocate vehicle to dealer
        inventory.DealerId = request.DealerId;
        inventory.AllocatedToDealerDate = DateTime.UtcNow;
        // Keep status as Available (not Reserved) since it's not assigned to an Order yet
        // Status will change to Reserved when allocated to an Order

        await _unitOfWork.VehicleInventories.UpdateAsync(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


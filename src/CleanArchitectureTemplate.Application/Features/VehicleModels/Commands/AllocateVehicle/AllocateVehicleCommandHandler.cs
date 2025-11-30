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
        // Get inventory
        var inventory = await _unitOfWork.VehicleInventories.GetByIdWithDetailsAsync(request.InventoryId)
            ?? throw new NotFoundException($"Vehicle inventory with ID {request.InventoryId} not found");

        // Get order
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(request.OrderId)
            ?? throw new NotFoundException($"Order with ID {request.OrderId} not found");

        // Business rule: Only available vehicles can be allocated
        if (inventory.Status != VehicleStatus.Available)
        {
            throw new ValidationException($"Vehicle is not available for allocation. Current status: {inventory.Status}");
        }

        // Business rule: Order must be in Approved status to allocate vehicle
        if (order.Status != OrderStatus.Approved)
        {
            throw new ValidationException($"Order must be in Approved status to allocate vehicle. Current status: {order.Status}");
        }

        // Business rule: Order already has a vehicle allocated
        if (order.VehicleInventoryId.HasValue)
        {
            throw new ValidationException($"Order already has a vehicle allocated. Vehicle ID: {order.VehicleInventoryId}");
        }

        // Business rule: Inventory variant and color must match order requirements
        if (inventory.VariantId != order.VehicleVariantId)
        {
            throw new ValidationException($"Vehicle variant does not match order requirement. Order requires variant ID: {order.VehicleVariantId}, but inventory has variant ID: {inventory.VariantId}");
        }

        if (inventory.ColorId != order.VehicleColorId)
        {
            throw new ValidationException($"Vehicle color does not match order requirement. Order requires color ID: {order.VehicleColorId}, but inventory has color ID: {inventory.ColorId}");
        }

        // Allocate vehicle to order
        order.VehicleInventoryId = inventory.Id;
        order.Status = OrderStatus.VehicleAllocated;

        // Update inventory
        inventory.Status = VehicleStatus.Reserved;
        inventory.DealerId = order.DealerId;
        inventory.AllocatedToDealerDate = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

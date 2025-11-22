using MediatR;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleInventoryStatus;

public class UpdateVehicleInventoryStatusCommandHandler : IRequestHandler<UpdateVehicleInventoryStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleInventoryStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateVehicleInventoryStatusCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _unitOfWork.VehicleInventories.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle inventory with ID {request.Id} not found");

        if (!Enum.TryParse<VehicleStatus>(request.Status, out var status))
        {
            throw new ValidationException("Invalid vehicle status");
        }

        // Business rule: Cannot change status from Sold
        if (inventory.Status == VehicleStatus.Sold)
        {
            throw new ValidationException("Cannot change status of sold vehicles");
        }

        inventory.Status = status;

        if (!string.IsNullOrEmpty(request.WarehouseLocation))
        {
            if (!Enum.TryParse<WarehouseLocation>(request.WarehouseLocation, out var location))
            {
                throw new ValidationException("Invalid warehouse location");
            }
            inventory.WarehouseLocation = location;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

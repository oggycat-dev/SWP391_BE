using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicle;

public class AllocateVehicleCommandValidator : AbstractValidator<AllocateVehicleCommand>
{
    public AllocateVehicleCommandValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty().WithMessage("Inventory ID is required");

        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required");
    }
}

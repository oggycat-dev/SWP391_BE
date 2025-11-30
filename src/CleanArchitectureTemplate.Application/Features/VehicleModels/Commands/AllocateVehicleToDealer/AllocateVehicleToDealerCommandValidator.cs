using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicleToDealer;

public class AllocateVehicleToDealerCommandValidator : AbstractValidator<AllocateVehicleToDealerCommand>
{
    public AllocateVehicleToDealerCommandValidator()
    {
        RuleFor(x => x.InventoryId)
            .NotEmpty().WithMessage("Inventory ID is required");

        RuleFor(x => x.DealerId)
            .NotEmpty().WithMessage("Dealer ID is required");
    }
}


using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleInventoryStatus;

public class UpdateVehicleInventoryStatusCommandValidator : AbstractValidator<UpdateVehicleInventoryStatusCommand>
{
    public UpdateVehicleInventoryStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required");
    }
}

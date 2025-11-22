using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleVariant;

public class UpdateVehicleVariantCommandValidator : AbstractValidator<UpdateVehicleVariantCommand>
{
    public UpdateVehicleVariantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.VariantName)
            .NotEmpty().WithMessage("Variant name is required")
            .MaximumLength(100).WithMessage("Variant name must not exceed 100 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

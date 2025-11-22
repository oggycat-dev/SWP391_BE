using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleVariant;

public class CreateVehicleVariantCommandValidator : AbstractValidator<CreateVehicleVariantCommand>
{
    public CreateVehicleVariantCommandValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("Model ID is required");

        RuleFor(x => x.VariantName)
            .NotEmpty().WithMessage("Variant name is required")
            .MaximumLength(100).WithMessage("Variant name must not exceed 100 characters");

        RuleFor(x => x.VariantCode)
            .NotEmpty().WithMessage("Variant code is required")
            .MaximumLength(50).WithMessage("Variant code must not exceed 50 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleModel;

public class CreateVehicleModelCommandValidator : AbstractValidator<CreateVehicleModelCommand>
{
    public CreateVehicleModelCommandValidator()
    {
        RuleFor(x => x.ModelCode)
            .NotEmpty().WithMessage("Model code is required")
            .MaximumLength(50).WithMessage("Model code must not exceed 50 characters");

        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("Model name is required")
            .MaximumLength(200).WithMessage("Model name must not exceed 200 characters");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required")
            .MaximumLength(100).WithMessage("Brand must not exceed 100 characters");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required");

        RuleFor(x => x.Year)
            .GreaterThan(2000).WithMessage("Year must be greater than 2000")
            .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Year cannot be in the future");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than 0");
    }
}

using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleModel;

public class UpdateVehicleModelCommandValidator : AbstractValidator<UpdateVehicleModelCommand>
{
    public UpdateVehicleModelCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("Model name is required")
            .MaximumLength(200).WithMessage("Model name must not exceed 200 characters");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Brand is required")
            .MaximumLength(100).WithMessage("Brand must not exceed 100 characters");

        RuleFor(x => x.Year)
            .GreaterThan(2000).WithMessage("Year must be greater than 2000");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than 0");
    }
}

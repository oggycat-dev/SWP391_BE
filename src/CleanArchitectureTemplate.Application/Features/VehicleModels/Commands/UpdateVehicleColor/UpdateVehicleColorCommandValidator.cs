using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleColor;

public class UpdateVehicleColorCommandValidator : AbstractValidator<UpdateVehicleColorCommand>
{
    public UpdateVehicleColorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");

        RuleFor(x => x.ColorName)
            .NotEmpty().WithMessage("Color name is required")
            .MaximumLength(50).WithMessage("Color name must not exceed 50 characters");

        RuleFor(x => x.ColorCode)
            .NotEmpty().WithMessage("Color code is required")
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("Color code must be in format #RRGGBB");

        RuleFor(x => x.AdditionalPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Additional price must be greater than or equal to 0");
    }
}

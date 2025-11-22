using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleInventory;

public class CreateVehicleInventoryCommandValidator : AbstractValidator<CreateVehicleInventoryCommand>
{
    public CreateVehicleInventoryCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty().WithMessage("Variant ID is required");

        RuleFor(x => x.ColorId)
            .NotEmpty().WithMessage("Color ID is required");

        RuleFor(x => x.VIN)
            .NotEmpty().WithMessage("VIN is required")
            .Length(17).WithMessage("VIN must be exactly 17 characters");

        RuleFor(x => x.ChassisNumber)
            .NotEmpty().WithMessage("Chassis number is required")
            .MaximumLength(50).WithMessage("Chassis number must not exceed 50 characters");

        RuleFor(x => x.EngineNumber)
            .NotEmpty().WithMessage("Engine number is required")
            .MaximumLength(50).WithMessage("Engine number must not exceed 50 characters");

        RuleFor(x => x.ManufactureDate)
            .NotEmpty().WithMessage("Manufacture date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Manufacture date cannot be in the future");

        RuleFor(x => x.ImportDate)
            .NotEmpty().WithMessage("Import date is required")
            .GreaterThanOrEqualTo(x => x.ManufactureDate).WithMessage("Import date must be after manufacture date");

        RuleFor(x => x.WarehouseLocation)
            .NotEmpty().WithMessage("Warehouse location is required");
    }
}

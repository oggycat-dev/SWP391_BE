using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Commands.CreateTestDrive;

public class CreateTestDriveCommandValidator : AbstractValidator<CreateTestDriveCommand>
{
    public CreateTestDriveCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.VehicleVariantId)
            .NotEmpty().WithMessage("Vehicle Variant ID is required");

        RuleFor(x => x.DealerId)
            .NotEmpty().WithMessage("Dealer ID is required");

        RuleFor(x => x.ScheduledDate)
            .NotEmpty().WithMessage("Scheduled date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future");
    }
}


using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Commands.CreateQuotation;

public class CreateQuotationCommandValidator : AbstractValidator<CreateQuotationCommand>
{
    public CreateQuotationCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.VehicleVariantId)
            .NotEmpty().WithMessage("Vehicle variant is required.");

        RuleFor(x => x.VehicleColorId)
            .NotEmpty().WithMessage("Vehicle color is required.");

        RuleFor(x => x.ValidityDays)
            .GreaterThan(0).WithMessage("Validity days must be greater than 0.")
            .LessThanOrEqualTo(90).WithMessage("Validity days cannot exceed 90 days.");

        RuleFor(x => x.DealerDiscount)
            .GreaterThanOrEqualTo(0).WithMessage("Dealer discount cannot be negative.");
    }
}


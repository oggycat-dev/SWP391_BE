using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.UpdateDealer;

public class UpdateDealerCommandValidator : AbstractValidator<UpdateDealerCommand>
{
    public UpdateDealerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Dealer ID is required");

        RuleFor(x => x.DealerName)
            .NotEmpty().WithMessage("Dealer name is required")
            .MaximumLength(200).WithMessage("Dealer name must not exceed 200 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required")
            .MaximumLength(100).WithMessage("District must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\d{10,11}$").WithMessage("Phone number must be 10-11 digits");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.DebtLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Debt limit must be greater than or equal to 0");
    }
}


using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.CreateDealer;

public class CreateDealerCommandValidator : AbstractValidator<CreateDealerCommand>
{
    public CreateDealerCommandValidator()
    {
        RuleFor(x => x.DealerCode)
            .NotEmpty().WithMessage("Dealer code is required")
            .MaximumLength(20).WithMessage("Dealer code must not exceed 20 characters");

        RuleFor(x => x.DealerName)
            .NotEmpty().WithMessage("Dealer name is required")
            .MaximumLength(200).WithMessage("Dealer name must not exceed 200 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9]{10,11}$").WithMessage("Phone number must be 10-11 digits");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.DebtLimit)
            .GreaterThanOrEqualTo(0).WithMessage("Debt limit must be greater than or equal to 0");
    }
}

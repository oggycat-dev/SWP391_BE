using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.CreateDealerContract;

public class CreateDealerContractCommandValidator : AbstractValidator<CreateDealerContractCommand>
{
    public CreateDealerContractCommandValidator()
    {
        RuleFor(x => x.DealerId)
            .NotEmpty().WithMessage("Dealer ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.Terms)
            .NotEmpty().WithMessage("Terms are required")
            .MaximumLength(2000).WithMessage("Terms cannot exceed 2000 characters");

        RuleFor(x => x.CommissionRate)
            .GreaterThanOrEqualTo(0).WithMessage("Commission rate must be non-negative")
            .LessThanOrEqualTo(100).WithMessage("Commission rate cannot exceed 100%");
    }
}


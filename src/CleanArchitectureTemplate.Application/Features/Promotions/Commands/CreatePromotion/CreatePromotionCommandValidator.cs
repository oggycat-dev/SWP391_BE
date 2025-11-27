using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("EndDate is required")
            .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate");

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0, 100)
            .When(x => x.DiscountType == Domain.Enums.DiscountType.Percentage)
            .WithMessage("DiscountPercentage must be between 0 and 100");

        RuleFor(x => x.DiscountAmount)
            .GreaterThan(0)
            .When(x => x.DiscountType == Domain.Enums.DiscountType.FixedAmount)
            .WithMessage("DiscountAmount must be greater than 0");

        RuleFor(x => x.MaxUsageCount)
            .GreaterThan(0).WithMessage("MaxUsageCount must be greater than 0");
    }
}


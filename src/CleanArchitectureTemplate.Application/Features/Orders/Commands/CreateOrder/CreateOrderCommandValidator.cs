using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.VehicleVariantId)
            .NotEmpty().WithMessage("Vehicle variant is required.");

        RuleFor(x => x.VehicleColorId)
            .NotEmpty().WithMessage("Vehicle color is required.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method.");

        RuleFor(x => x.DealerDiscount)
            .GreaterThanOrEqualTo(0).WithMessage("Dealer discount cannot be negative.");
    }
}


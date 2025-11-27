using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CreateDelivery;

public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
{
    public CreateDeliveryCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required");

        RuleFor(x => x.ScheduledDate)
            .NotEmpty().WithMessage("ScheduledDate is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("ScheduledDate must be in the future");

        RuleFor(x => x.DeliveryAddress)
            .NotEmpty().WithMessage("DeliveryAddress is required")
            .MaximumLength(500).WithMessage("DeliveryAddress must not exceed 500 characters");

        RuleFor(x => x.ReceiverName)
            .NotEmpty().WithMessage("ReceiverName is required")
            .MaximumLength(200).WithMessage("ReceiverName must not exceed 200 characters");

        RuleFor(x => x.ReceiverPhone)
            .NotEmpty().WithMessage("ReceiverPhone is required")
            .MaximumLength(20).WithMessage("ReceiverPhone must not exceed 20 characters");
    }
}


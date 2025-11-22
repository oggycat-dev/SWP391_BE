using FluentValidation;

namespace CleanArchitectureTemplate.Application.Features.SalesContracts.Commands.CreateSalesContract;

public class CreateSalesContractCommandValidator : AbstractValidator<CreateSalesContractCommand>
{
    public CreateSalesContractCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required");

        RuleFor(x => x.ContractDate)
            .NotEmpty().WithMessage("Contract date is required");
    }
}


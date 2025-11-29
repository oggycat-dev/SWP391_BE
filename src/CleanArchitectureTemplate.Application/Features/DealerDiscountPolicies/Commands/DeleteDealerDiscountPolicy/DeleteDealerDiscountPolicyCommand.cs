using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.DeleteDealerDiscountPolicy;

/// <summary>
/// Delete (deactivate) dealer discount policy command
/// </summary>
public class DeleteDealerDiscountPolicyCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}


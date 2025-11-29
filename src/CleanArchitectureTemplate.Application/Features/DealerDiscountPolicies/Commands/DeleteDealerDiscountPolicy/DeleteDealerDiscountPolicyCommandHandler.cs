using MediatR;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.DeleteDealerDiscountPolicy;

public class DeleteDealerDiscountPolicyCommandHandler : IRequestHandler<DeleteDealerDiscountPolicyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDealerDiscountPolicyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDealerDiscountPolicyCommand request, CancellationToken cancellationToken)
    {
        var policy = await _unitOfWork.DealerDiscountPolicies.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Dealer discount policy with ID {request.Id} not found");

        // Soft delete: Deactivate the policy
        policy.IsActive = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}


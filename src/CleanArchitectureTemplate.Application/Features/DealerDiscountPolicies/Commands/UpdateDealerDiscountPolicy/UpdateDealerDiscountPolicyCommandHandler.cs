using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.UpdateDealerDiscountPolicy;

public class UpdateDealerDiscountPolicyCommandHandler : IRequestHandler<UpdateDealerDiscountPolicyCommand, DealerDiscountPolicyDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDealerDiscountPolicyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDiscountPolicyDto> Handle(UpdateDealerDiscountPolicyCommand request, CancellationToken cancellationToken)
    {
        var policy = await _unitOfWork.DealerDiscountPolicies.GetByIdAsync(request.Id);
        if (policy == null)
        {
            throw new KeyNotFoundException($"Dealer discount policy with ID {request.Id} not found");
        }

        policy.DiscountRate = request.DiscountRate;
        policy.MinOrderQuantity = request.MinOrderQuantity;
        policy.MaxDiscountAmount = request.MaxDiscountAmount;
        policy.EffectiveDate = request.EffectiveDate;
        policy.ExpiryDate = request.ExpiryDate;
        policy.IsActive = request.IsActive;
        policy.Conditions = request.Conditions;
        policy.ModifiedAt = DateTime.UtcNow;

        await _unitOfWork.DealerDiscountPolicies.UpdateAsync(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dealer = await _unitOfWork.Dealers.GetByIdAsync(policy.DealerId);
        var result = _mapper.Map<DealerDiscountPolicyDto>(policy);
        result.DealerName = dealer?.Name ?? string.Empty;
        
        if (policy.VehicleVariantId.HasValue)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(policy.VehicleVariantId.Value);
            result.VehicleVariantName = variant?.VariantName ?? string.Empty;
        }
        
        return result;
    }
}


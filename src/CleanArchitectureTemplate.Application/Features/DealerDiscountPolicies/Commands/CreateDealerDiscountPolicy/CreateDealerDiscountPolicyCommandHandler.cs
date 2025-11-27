using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.CreateDealerDiscountPolicy;

public class CreateDealerDiscountPolicyCommandHandler : IRequestHandler<CreateDealerDiscountPolicyCommand, DealerDiscountPolicyDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDealerDiscountPolicyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDiscountPolicyDto> Handle(CreateDealerDiscountPolicyCommand request, CancellationToken cancellationToken)
    {
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId);
        if (dealer == null)
        {
            throw new KeyNotFoundException($"Dealer with ID {request.DealerId} not found");
        }

        if (request.VehicleVariantId.HasValue)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VehicleVariantId.Value);
            if (variant == null)
            {
                throw new KeyNotFoundException($"Vehicle variant with ID {request.VehicleVariantId} not found");
            }
        }

        var policy = new DealerDiscountPolicy
        {
            Id = Guid.NewGuid(),
            DealerId = request.DealerId,
            VehicleVariantId = request.VehicleVariantId,
            DiscountRate = request.DiscountRate,
            MinOrderQuantity = request.MinOrderQuantity,
            MaxDiscountAmount = request.MaxDiscountAmount,
            EffectiveDate = request.EffectiveDate,
            ExpiryDate = request.ExpiryDate,
            IsActive = true,
            Conditions = request.Conditions,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DealerDiscountPolicies.AddAsync(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<DealerDiscountPolicyDto>(policy);
        result.DealerName = dealer.Name;
        
        if (request.VehicleVariantId.HasValue)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VehicleVariantId.Value);
            result.VehicleVariantName = variant?.VariantName ?? string.Empty;
        }
        
        return result;
    }
}


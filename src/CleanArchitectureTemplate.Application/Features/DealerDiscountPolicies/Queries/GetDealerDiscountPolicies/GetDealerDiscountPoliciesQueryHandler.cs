using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Queries.GetDealerDiscountPolicies;

public class GetDealerDiscountPoliciesQueryHandler : IRequestHandler<GetDealerDiscountPoliciesQuery, List<DealerDiscountPolicyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerDiscountPoliciesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<DealerDiscountPolicyDto>> Handle(GetDealerDiscountPoliciesQuery request, CancellationToken cancellationToken)
    {
        var policies = request.DealerId.HasValue
            ? await _unitOfWork.DealerDiscountPolicies.GetByDealerIdAsync(request.DealerId.Value)
            : (request.ActiveOnly == true 
                ? await _unitOfWork.DealerDiscountPolicies.GetActivePolicesAsync()
                : await _unitOfWork.DealerDiscountPolicies.GetAllAsync()).ToList();

        if (request.ActiveOnly == true && !request.DealerId.HasValue)
        {
            policies = policies.Where(p => p.IsActive).ToList();
        }

        var dealers = await _unitOfWork.Dealers.GetAllAsync();
        var dealerDict = dealers.ToDictionary(d => d.Id, d => d.Name);

        var variants = await _unitOfWork.VehicleVariants.GetAllAsync();
        var variantDict = variants.ToDictionary(v => v.Id, v => v.VariantName);

        var result = policies.Select(p =>
        {
            var dto = _mapper.Map<DealerDiscountPolicyDto>(p);
            dto.DealerName = dealerDict.TryGetValue(p.DealerId, out var dealerName) ? dealerName : string.Empty;
            dto.VehicleVariantName = p.VehicleVariantId.HasValue && variantDict.TryGetValue(p.VehicleVariantId.Value, out var variantName)
                ? variantName
                : string.Empty;
            return dto;
        }).OrderByDescending(p => p.IsActive).ThenByDescending(p => p.EffectiveDate).ToList();

        return result;
    }
}


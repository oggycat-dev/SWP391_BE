using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Queries.GetDealerDiscountPolicies;

public class GetDealerDiscountPoliciesQuery : IRequest<List<DealerDiscountPolicyDto>>
{
    public Guid? DealerId { get; set; }
    public bool? ActiveOnly { get; set; }
}


using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Queries.GetDealerDiscountPolicyById;

public class GetDealerDiscountPolicyByIdQuery : IRequest<DealerDiscountPolicyDto>
{
    public Guid Id { get; set; }
}


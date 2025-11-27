using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.UpdateDealerDiscountPolicy;

public class UpdateDealerDiscountPolicyCommand : IRequest<DealerDiscountPolicyDto>
{
    public Guid Id { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal MinOrderQuantity { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string Conditions { get; set; } = string.Empty;
}


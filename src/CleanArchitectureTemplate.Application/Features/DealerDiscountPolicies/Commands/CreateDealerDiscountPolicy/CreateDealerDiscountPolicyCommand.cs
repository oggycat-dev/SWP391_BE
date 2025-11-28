using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.CreateDealerDiscountPolicy;

public class CreateDealerDiscountPolicyCommand : IRequest<DealerDiscountPolicyDto>
{
    public Guid DealerId { get; set; }
    public Guid? VehicleVariantId { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal MinOrderQuantity { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Conditions { get; set; } = string.Empty;
}


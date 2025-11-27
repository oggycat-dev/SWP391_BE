using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommand : IRequest<PromotionDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public List<Guid> ApplicableVehicleVariantIds { get; set; } = new();
    public List<Guid> ApplicableDealerIds { get; set; } = new();
    public int MaxUsageCount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty;
}


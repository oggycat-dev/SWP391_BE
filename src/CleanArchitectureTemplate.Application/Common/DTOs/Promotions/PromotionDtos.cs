using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Promotions;

public class PromotionDto
{
    public Guid Id { get; set; }
    public string PromotionCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public PromotionStatus Status { get; set; }
    public List<Guid> ApplicableVehicleVariantIds { get; set; } = new();
    public List<Guid> ApplicableDealerIds { get; set; } = new();
    public int MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreatePromotionRequest
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

public class UpdatePromotionRequest
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

public class UpdatePromotionStatusRequest
{
    public PromotionStatus Status { get; set; }
}


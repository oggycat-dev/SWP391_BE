using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Chương trình khuyến mãi
/// </summary>
public class Promotion : BaseEntity
{
    public string PromotionCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public DiscountType DiscountType { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    
    public PromotionStatus Status { get; set; } = PromotionStatus.Active;
    
    /// <summary>
    /// Applicable vehicle variant IDs (JSON array)
    /// Example: ["guid1", "guid2"]
    /// </summary>
    public string ApplicableVehicleVariantIds { get; set; } = string.Empty;
    
    /// <summary>
    /// Applicable dealer IDs (JSON array). Empty = all dealers
    /// </summary>
    public string ApplicableDealerIds { get; set; } = string.Empty;
    
    public int MaxUsageCount { get; set; } // Số lần sử dụng tối đa
    public int CurrentUsageCount { get; set; } // Đã sử dụng bao nhiêu lần
    
    public string ImageUrl { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty;
    
    // Navigation properties
    public ICollection<OrderPromotion> OrderPromotions { get; set; } = new List<OrderPromotion>();
}

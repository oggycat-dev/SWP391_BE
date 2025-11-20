using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Chính sách chiết khấu cho đại lý
/// </summary>
public class DealerDiscountPolicy : BaseEntity
{
    public Guid DealerId { get; set; }
    public Guid? VehicleVariantId { get; set; } // null = áp dụng cho tất cả xe
    
    public decimal DiscountRate { get; set; } // %
    public decimal MinOrderQuantity { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    public string Conditions { get; set; } = string.Empty; // JSON
    
    // Navigation properties
    public Dealer Dealer { get; set; } = null!;
    public VehicleVariant? VehicleVariant { get; set; }
}

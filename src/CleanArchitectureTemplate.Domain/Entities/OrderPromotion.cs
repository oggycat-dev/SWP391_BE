using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Liên kết giữa Order và Promotion
/// </summary>
public class OrderPromotion : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid PromotionId { get; set; }
    public decimal DiscountAmount { get; set; }
    
    // Navigation properties
    public Order Order { get; set; } = null!;
    public Promotion Promotion { get; set; } = null!;
}

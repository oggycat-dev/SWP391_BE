using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Báo giá
/// </summary>
public class Quotation : BaseEntity
{
    public string QuotationNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Guid DealerId { get; set; }
    public Guid DealerStaffId { get; set; }
    
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    
    public decimal BasePrice { get; set; }
    public decimal ColorPrice { get; set; }
    public decimal DealerDiscount { get; set; }
    public decimal PromotionDiscount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal QuotedPrice { get; set; }
    public decimal TotalAmount { get; set; } // Final total after all calculations
    
    public DateTime QuotationDate { get; set; }
    public DateTime ValidUntil { get; set; }
    public QuotationStatus Status { get; set; } = QuotationStatus.Draft;
    
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Dealer Dealer { get; set; } = null!;
    public DealerStaff DealerStaff { get; set; } = null!;
    public VehicleVariant VehicleVariant { get; set; } = null!;
    public VehicleColor VehicleColor { get; set; } = null!;
    public Order? Order { get; set; } // Một báo giá chỉ tạo được 1 order
}

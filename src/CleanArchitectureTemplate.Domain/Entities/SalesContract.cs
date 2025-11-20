using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Hợp đồng bán hàng
/// </summary>
public class SalesContract : BaseEntity
{
    public Guid OrderId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    
    public DateTime SignedDate { get; set; }
    public string Terms { get; set; } = string.Empty; // Điều khoản hợp đồng (JSON)
    public decimal TotalAmount { get; set; }
    
    public string CustomerSignature { get; set; } = string.Empty; // Base64 image
    public string DealerSignature { get; set; } = string.Empty;
    
    public string ContractFileUrl { get; set; } = string.Empty; // PDF
    
    // Navigation properties
    public Order Order { get; set; } = null!;
}

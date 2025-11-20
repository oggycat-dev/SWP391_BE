using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Hợp đồng đại lý
/// </summary>
public class DealerContract : BaseEntity
{
    public Guid DealerId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public string Terms { get; set; } = string.Empty; // Điều khoản hợp đồng
    public decimal CommissionRate { get; set; } // Tỷ lệ hoa hồng (%)
    
    public DealerContractStatus Status { get; set; } = DealerContractStatus.Draft;
    
    public string SignedBy { get; set; } = string.Empty;
    public DateTime? SignedDate { get; set; }
    
    // Navigation properties
    public Dealer Dealer { get; set; } = null!;
}

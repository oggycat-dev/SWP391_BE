using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Nhân viên đại lý
/// </summary>
public class DealerStaff : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid DealerId { get; set; }
    
    public string Position { get; set; } = string.Empty;
    public decimal SalesTarget { get; set; }
    public decimal CurrentSales { get; set; }
    public decimal CommissionRate { get; set; }
    
    public DateTime JoinedDate { get; set; }
    public DateTime? LeftDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Dealer Dealer { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

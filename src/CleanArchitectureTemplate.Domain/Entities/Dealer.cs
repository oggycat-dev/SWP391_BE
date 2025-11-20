using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Đại lý
/// </summary>
public class Dealer : BaseEntity
{
    public string DealerCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    
    public DealerStatus Status { get; set; } = DealerStatus.Active;
    
    public decimal SalesTarget { get; set; }
    public decimal CurrentSales { get; set; }
    public decimal DebtLimit { get; set; }
    public decimal CurrentDebt { get; set; }
    
    // Navigation properties
    public ICollection<DealerContract> Contracts { get; set; } = new List<DealerContract>();
    public ICollection<DealerStaff> Staff { get; set; } = new List<DealerStaff>();
    public ICollection<VehicleInventory> Inventories { get; set; } = new List<VehicleInventory>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<DealerDebt> Debts { get; set; } = new List<DealerDebt>();
}

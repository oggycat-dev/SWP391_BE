using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Công nợ khách hàng
/// </summary>
public class CustomerDebt : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public string DebtCode { get; set; } = string.Empty;
    
    public decimal TotalDebt { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    
    public DateTime DueDate { get; set; }
    public DebtStatus Status { get; set; } = DebtStatus.Current;
    
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Order Order { get; set; } = null!;
}

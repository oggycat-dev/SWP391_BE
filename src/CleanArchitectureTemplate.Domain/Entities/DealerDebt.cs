using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Công nợ đại lý với hãng xe
/// </summary>
public class DealerDebt : BaseEntity
{
    public Guid DealerId { get; set; }
    public string DebtCode { get; set; } = string.Empty;
    
    public decimal TotalDebt { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    
    public DateTime DueDate { get; set; }
    public DebtStatus Status { get; set; } = DebtStatus.Current;
    
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public Dealer Dealer { get; set; } = null!;
}

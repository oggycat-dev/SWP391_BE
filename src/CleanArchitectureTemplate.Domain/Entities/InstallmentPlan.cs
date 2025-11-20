using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Gói trả góp
/// </summary>
public class InstallmentPlan : BaseEntity
{
    public Guid OrderId { get; set; }
    public string PlanCode { get; set; } = string.Empty;
    
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    
    public decimal TotalAmount { get; set; }
    public decimal DownPayment { get; set; }
    public decimal LoanAmount { get; set; }
    
    public int TermMonths { get; set; } // 12, 24, 36, 48
    public decimal InterestRate { get; set; } // %/year
    public decimal MonthlyPayment { get; set; }
    
    public DateTime FirstPaymentDate { get; set; }
    public DateTime LastPaymentDate { get; set; }
    
    public string BankContractNumber { get; set; } = string.Empty;
    public DateTime? BankApprovedDate { get; set; }
    
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public Order Order { get; set; } = null!;
}

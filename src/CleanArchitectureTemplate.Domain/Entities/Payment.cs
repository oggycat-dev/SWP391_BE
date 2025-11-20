using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Thanh toán
/// </summary>
public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public string PaymentCode { get; set; } = string.Empty;
    
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    public string TransactionReference { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    
    public string ReceiptUrl { get; set; } = string.Empty; // PDF receipt
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public Order Order { get; set; } = null!;
}

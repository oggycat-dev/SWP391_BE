using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Payments;

public class PaymentDto
{
    public Guid Id { get; set; }
    public string PaymentCode { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionReference { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreatePaymentRequest
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public string? TransactionReference { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class InstallmentPlanDto
{
    public Guid Id { get; set; }
    public string PlanCode { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DownPayment { get; set; }
    public decimal LoanAmount { get; set; }
    public int TermMonths { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime NextPaymentDate { get; set; }
    public string BankName { get; set; } = string.Empty;
}

public class CreateInstallmentPlanRequest
{
    public Guid OrderId { get; set; }
    public decimal DownPayment { get; set; }
    public int TermMonths { get; set; }
    public decimal InterestRate { get; set; }
    public string BankName { get; set; } = string.Empty;
}


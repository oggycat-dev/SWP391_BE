namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerDebtDto
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid? OrderId { get; set; }
    public string DebtType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateDealerDebtRequest
{
    public Guid DealerId { get; set; }
    public Guid? OrderId { get; set; }
    public string DebtType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
}

public class RecordDebtPaymentRequest
{
    public decimal PaymentAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Notes { get; set; }
}

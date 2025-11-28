using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerDebtDto
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string DebtCode { get; set; } = string.Empty;
    public decimal TotalDebt { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public DebtStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateDealerDebtRequest
{
    public Guid DealerId { get; set; }
    public decimal TotalDebt { get; set; }
    public DateTime DueDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class PayDealerDebtRequest
{
    public decimal PaymentAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
}

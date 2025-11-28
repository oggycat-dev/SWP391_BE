using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerContractDto
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Terms { get; set; } = string.Empty;
    public decimal CommissionRate { get; set; }
    public DealerContractStatus Status { get; set; }
    public string SignedBy { get; set; } = string.Empty;
    public DateTime? SignedDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDealerContractRequest
{
    public Guid DealerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Terms { get; set; } = string.Empty;
    public decimal CommissionRate { get; set; }
}

public class UpdateDealerContractStatusRequest
{
    public DealerContractStatus Status { get; set; }
    public string? SignedBy { get; set; }
}

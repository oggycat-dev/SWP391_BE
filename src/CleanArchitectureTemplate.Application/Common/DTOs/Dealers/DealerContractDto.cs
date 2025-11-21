namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerContractDto
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal CommissionRate { get; set; }
    public string TermsAndConditions { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateDealerContractRequest
{
    public Guid DealerId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal CommissionRate { get; set; }
    public string TermsAndConditions { get; set; } = string.Empty;
}

public class UpdateDealerContractRequest
{
    public DateTime EndDate { get; set; }
    public decimal CommissionRate { get; set; }
    public string TermsAndConditions { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

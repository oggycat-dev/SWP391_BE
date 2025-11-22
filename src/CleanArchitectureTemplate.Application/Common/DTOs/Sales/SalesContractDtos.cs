namespace CleanArchitectureTemplate.Application.Common.DTOs.Sales;

public class SalesContractDto
{
    public Guid Id { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public DateTime ContractDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? SpecialTerms { get; set; }
    public DateTime CreatedAt { get; set; }
}


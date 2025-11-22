using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Quotations;

public class QuotationDto
{
    public Guid Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid VehicleVariantId { get; set; }
    public string VehicleVariantName { get; set; } = string.Empty;
    public Guid VehicleColorId { get; set; }
    public string VehicleColorName { get; set; } = string.Empty;
    public QuotationStatus Status { get; set; }
    public DateTime QuotationDate { get; set; }
    public DateTime ValidUntil { get; set; }
    public decimal BasePrice { get; set; }
    public decimal ColorPrice { get; set; }
    public decimal DealerDiscount { get; set; }
    public decimal PromotionDiscount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreateQuotationRequest
{
    public Guid CustomerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    public int ValidityDays { get; set; } = 30;
    public decimal DealerDiscount { get; set; }
    public List<Guid> PromotionIds { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public class ApproveQuotationRequest
{
    public bool Approved { get; set; }
    public string? RejectionReason { get; set; }
}


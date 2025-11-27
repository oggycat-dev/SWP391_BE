namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerDiscountPolicyDto
{
    public Guid Id { get; set; }
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid? VehicleVariantId { get; set; }
    public string VehicleVariantName { get; set; } = string.Empty;
    public decimal DiscountRate { get; set; }
    public decimal MinOrderQuantity { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string Conditions { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateDealerDiscountPolicyRequest
{
    public Guid DealerId { get; set; }
    public Guid? VehicleVariantId { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal MinOrderQuantity { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Conditions { get; set; } = string.Empty;
}

public class UpdateDealerDiscountPolicyRequest
{
    public decimal DiscountRate { get; set; }
    public decimal MinOrderQuantity { get; set; }
    public decimal MaxDiscountAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public string Conditions { get; set; } = string.Empty;
}


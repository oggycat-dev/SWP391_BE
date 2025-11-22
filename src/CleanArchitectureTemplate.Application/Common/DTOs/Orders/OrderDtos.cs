using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Orders;

public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid VehicleVariantId { get; set; }
    public string VehicleVariantName { get; set; } = string.Empty;
    public string VehicleModelName { get; set; } = string.Empty;
    public Guid VehicleColorId { get; set; }
    public string VehicleColorName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public decimal BasePrice { get; set; }
    public decimal ColorPrice { get; set; }
    public decimal DealerDiscount { get; set; }
    public decimal PromotionDiscount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool IsInstallment { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreateOrderRequest
{
    public Guid? QuotationId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public bool IsInstallment { get; set; }
    public decimal DealerDiscount { get; set; }
    public List<Guid> PromotionIds { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}

public class UpdateOrderStatusRequest
{
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class AllocateVehicleToOrderRequest
{
    public Guid VehicleInventoryId { get; set; }
}


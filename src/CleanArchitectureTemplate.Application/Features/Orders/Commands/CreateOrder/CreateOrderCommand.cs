using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<OrderDto>
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


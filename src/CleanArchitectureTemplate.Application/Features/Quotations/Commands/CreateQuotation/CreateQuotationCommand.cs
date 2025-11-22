using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Commands.CreateQuotation;

public class CreateQuotationCommand : IRequest<QuotationDto>
{
    public Guid CustomerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    public int ValidityDays { get; set; } = 30;
    public decimal DealerDiscount { get; set; }
    public List<Guid> PromotionIds { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
}


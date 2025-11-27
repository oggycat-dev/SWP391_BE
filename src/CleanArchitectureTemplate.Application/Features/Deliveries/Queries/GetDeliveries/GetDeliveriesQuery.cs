using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveries;

public class GetDeliveriesQuery : IRequest<List<DeliveryDto>>
{
    public Guid? DealerId { get; set; }
    public DeliveryStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}


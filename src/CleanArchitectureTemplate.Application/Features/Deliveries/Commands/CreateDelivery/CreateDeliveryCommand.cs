using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CreateDelivery;

public class CreateDeliveryCommand : IRequest<DeliveryDto>
{
    public Guid OrderId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}


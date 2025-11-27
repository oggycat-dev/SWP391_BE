using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.UpdateDeliveryStatus;

public class UpdateDeliveryStatusCommand : IRequest<DeliveryDto>
{
    public Guid Id { get; set; }
    public DeliveryStatus Status { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}


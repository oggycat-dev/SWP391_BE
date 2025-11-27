using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveryById;

public class GetDeliveryByIdQuery : IRequest<DeliveryDto>
{
    public Guid Id { get; set; }
}


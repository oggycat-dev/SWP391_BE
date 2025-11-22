using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDto>
{
    public Guid Id { get; set; }
}


using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQuery : IRequest<List<OrderDto>>
{
    public Guid? DealerId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? DealerStaffId { get; set; }
    public OrderStatus? Status { get; set; }
}


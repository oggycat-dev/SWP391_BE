using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Application.Features.Orders.Commands.CreateOrder;
using CleanArchitectureTemplate.Application.Features.Orders.Commands.UpdateOrderStatus;
using CleanArchitectureTemplate.Application.Features.Orders.Queries.GetOrders;
using CleanArchitectureTemplate.Application.Features.Orders.Queries.GetOrderById;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing orders
/// </summary>
[ApiController]
[Route("api/dealer/orders")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all orders for the dealer
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetOrders(
        [FromQuery] Guid? customerId,
        [FromQuery] OrderStatus? status)
    {
        var query = new GetOrdersQuery 
        { 
            CustomerId = customerId,
            Status = status
        };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<OrderDto>>.Ok(result, "Orders retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(Guid id)
    {
        var query = new GetOrderByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<OrderDto>.Ok(result, "Order retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new order
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder(
        [FromBody] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<OrderDto>.Created(result, "Order created successfully");
        return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update order status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    [Authorize(Roles = "DealerManager")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrderStatus(
        Guid id,
        [FromBody] UpdateOrderStatusRequest request)
    {
        var command = new UpdateOrderStatusCommand
        {
            OrderId = id,
            Status = request.Status,
            Notes = request.Notes
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<OrderDto>.Ok(result, "Order status updated successfully");
        return Ok(response);
    }
}


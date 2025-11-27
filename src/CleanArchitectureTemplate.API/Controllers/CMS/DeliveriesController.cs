using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveries;
using CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveryById;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for viewing deliveries
/// </summary>
[ApiController]
[Route("api/cms/deliveries")]
[Authorize(Roles = "Admin,InventoryManager,SalesManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class DeliveriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all deliveries (admin view)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DeliveryDto>>>> GetDeliveries(
        [FromQuery] Guid? dealerId,
        [FromQuery] Domain.Enums.DeliveryStatus? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var query = new GetDeliveriesQuery
        {
            DealerId = dealerId,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate
        };
        
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<DeliveryDto>>.Ok(result, "Deliveries retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get delivery by ID (admin view)
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DeliveryDto>>> GetDeliveryById(Guid id)
    {
        var query = new GetDeliveryByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<DeliveryDto>.Ok(result, "Delivery retrieved successfully");
        return Ok(response);
    }
}


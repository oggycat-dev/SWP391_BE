using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CreateDelivery;
using CleanArchitectureTemplate.Application.Features.Deliveries.Commands.UpdateDeliveryStatus;
using CleanArchitectureTemplate.Application.Features.Deliveries.Commands.UploadPhotos;
using CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CaptureSignature;
using CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveries;
using CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveryById;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing deliveries
/// </summary>
[ApiController]
[Route("api/dealer/deliveries")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class DeliveriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeliveriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all deliveries for the dealer
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
    /// Get delivery by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DeliveryDto>>> GetDeliveryById(Guid id)
    {
        var query = new GetDeliveryByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<DeliveryDto>.Ok(result, "Delivery retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new delivery
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DeliveryDto>>> CreateDelivery(
        [FromBody] CreateDeliveryCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<DeliveryDto>.Created(result, "Delivery created successfully");
        return CreatedAtAction(nameof(GetDeliveryById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update delivery status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<DeliveryDto>>> UpdateDeliveryStatus(
        Guid id,
        [FromBody] UpdateDeliveryStatusRequest request)
    {
        var command = new UpdateDeliveryStatusCommand
        {
            Id = id,
            Status = request.Status,
            ActualDeliveryDate = request.ActualDeliveryDate,
            Notes = request.Notes
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DeliveryDto>.Ok(result, "Delivery status updated successfully");
        return Ok(response);
    }

    /// <summary>
    /// Upload delivery photos
    /// </summary>
    [HttpPost("{id:guid}/photos")]
    public async Task<ActionResult<ApiResponse<DeliveryDto>>> UploadPhotos(
        Guid id,
        [FromBody] UploadDeliveryPhotosRequest request)
    {
        var command = new UploadDeliveryPhotosCommand
        {
            Id = id,
            PhotoUrls = request.PhotoUrls
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DeliveryDto>.Ok(result, "Delivery photos uploaded successfully");
        return Ok(response);
    }

    /// <summary>
    /// Capture receiver signature
    /// </summary>
    [HttpPost("{id:guid}/signature")]
    public async Task<ActionResult<ApiResponse<DeliveryDto>>> CaptureSignature(
        Guid id,
        [FromBody] CaptureSignatureRequest request)
    {
        var command = new CaptureSignatureCommand
        {
            Id = id,
            SignatureBase64 = request.SignatureBase64
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DeliveryDto>.Ok(result, "Signature captured successfully");
        return Ok(response);
    }
}


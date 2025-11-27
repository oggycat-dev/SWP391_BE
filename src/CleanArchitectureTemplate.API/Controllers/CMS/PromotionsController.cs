using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Features.Promotions.Commands.CreatePromotion;
using CleanArchitectureTemplate.Application.Features.Promotions.Commands.UpdatePromotion;
using CleanArchitectureTemplate.Application.Features.Promotions.Commands.UpdatePromotionStatus;
using CleanArchitectureTemplate.Application.Features.Promotions.Commands.DeletePromotion;
using CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetPromotions;
using CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetPromotionById;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing promotions
/// </summary>
[ApiController]
[Route("api/cms/promotions")]
[Authorize(Roles = "Admin,SalesManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class PromotionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromotionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all promotions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PromotionDto>>>> GetPromotions(
        [FromQuery] Domain.Enums.PromotionStatus? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var query = new GetPromotionsQuery
        {
            Status = status,
            FromDate = fromDate,
            ToDate = toDate
        };
        
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<PromotionDto>>.Ok(result, "Promotions retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get promotion by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> GetPromotionById(Guid id)
    {
        var query = new GetPromotionByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<PromotionDto>.Ok(result, "Promotion retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new promotion
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> CreatePromotion(
        [FromBody] CreatePromotionCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<PromotionDto>.Created(result, "Promotion created successfully");
        return CreatedAtAction(nameof(GetPromotionById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update existing promotion
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> UpdatePromotion(
        Guid id,
        [FromBody] UpdatePromotionRequest request)
    {
        var command = new UpdatePromotionCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DiscountType = request.DiscountType,
            DiscountPercentage = request.DiscountPercentage,
            DiscountAmount = request.DiscountAmount,
            ApplicableVehicleVariantIds = request.ApplicableVehicleVariantIds,
            ApplicableDealerIds = request.ApplicableDealerIds,
            MaxUsageCount = request.MaxUsageCount,
            ImageUrl = request.ImageUrl,
            TermsAndConditions = request.TermsAndConditions
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<PromotionDto>.Ok(result, "Promotion updated successfully");
        return Ok(response);
    }

    /// <summary>
    /// Update promotion status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<PromotionDto>>> UpdatePromotionStatus(
        Guid id,
        [FromBody] UpdatePromotionStatusRequest request)
    {
        var command = new UpdatePromotionStatusCommand
        {
            Id = id,
            Status = request.Status
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<PromotionDto>.Ok(result, "Promotion status updated successfully");
        return Ok(response);
    }

    /// <summary>
    /// Delete promotion (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePromotion(Guid id)
    {
        var command = new DeletePromotionCommand { Id = id };
        await _mediator.Send(command);
        var response = ApiResponse<object>.Ok(null, "Promotion deleted successfully");
        return Ok(response);
    }
}


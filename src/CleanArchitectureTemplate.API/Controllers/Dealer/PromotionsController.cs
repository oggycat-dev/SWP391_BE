using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetActivePromotions;
using CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetPromotionById;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for viewing promotions
/// </summary>
[ApiController]
[Route("api/dealer/promotions")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class PromotionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromotionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get active promotions
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<List<PromotionDto>>>> GetActivePromotions()
    {
        var query = new GetActivePromotionsQuery();
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<PromotionDto>>.Ok(result, "Active promotions retrieved successfully");
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
}


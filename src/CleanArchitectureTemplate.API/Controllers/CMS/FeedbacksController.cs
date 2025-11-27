using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.RespondToFeedback;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbacks;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbackById;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing customer feedbacks
/// </summary>
[ApiController]
[Route("api/cms/feedbacks")]
[Authorize(Roles = "Admin,SalesManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class FeedbacksController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeedbacksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all feedbacks (admin view)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CustomerFeedbackDto>>>> GetFeedbacks(
        [FromQuery] Guid? dealerId,
        [FromQuery] Guid? customerId,
        [FromQuery] Domain.Enums.FeedbackStatus? status,
        [FromQuery] int? minRating,
        [FromQuery] int? maxRating)
    {
        var query = new GetFeedbacksQuery
        {
            DealerId = dealerId,
            CustomerId = customerId,
            Status = status,
            MinRating = minRating,
            MaxRating = maxRating
        };
        
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<CustomerFeedbackDto>>.Ok(result, "Feedbacks retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get feedback by ID (admin view)
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerFeedbackDto>>> GetFeedbackById(Guid id)
    {
        var query = new GetFeedbackByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<CustomerFeedbackDto>.Ok(result, "Feedback retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Respond to feedback (admin)
    /// </summary>
    [HttpPut("{id:guid}/respond")]
    public async Task<ActionResult<ApiResponse<CustomerFeedbackDto>>> RespondToFeedback(
        Guid id,
        [FromBody] RespondToFeedbackRequest request)
    {
        var command = new RespondToFeedbackCommand
        {
            Id = id,
            Response = request.Response
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<CustomerFeedbackDto>.Ok(result, "Response submitted successfully");
        return Ok(response);
    }
}


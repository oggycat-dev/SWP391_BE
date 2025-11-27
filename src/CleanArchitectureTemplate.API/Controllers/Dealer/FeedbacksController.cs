using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.SubmitFeedback;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.RespondToFeedback;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.UpdateStatus;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbacks;
using CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbackById;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing customer feedbacks
/// </summary>
[ApiController]
[Route("api/dealer/feedbacks")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class FeedbacksController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeedbacksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all feedbacks for the dealer
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
    /// Get feedback by ID
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
    /// Submit new feedback
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerFeedbackDto>>> SubmitFeedback(
        [FromBody] SubmitFeedbackCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<CustomerFeedbackDto>.Created(result, "Feedback submitted successfully");
        return CreatedAtAction(nameof(GetFeedbackById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Respond to feedback
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

    /// <summary>
    /// Update feedback status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<CustomerFeedbackDto>>> UpdateFeedbackStatus(
        Guid id,
        [FromBody] UpdateFeedbackStatusRequest request)
    {
        var command = new UpdateFeedbackStatusCommand
        {
            Id = id,
            FeedbackStatus = request.FeedbackStatus,
            ComplaintStatus = request.ComplaintStatus
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<CustomerFeedbackDto>.Ok(result, "Feedback status updated successfully");
        return Ok(response);
    }
}


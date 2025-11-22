using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using CleanArchitectureTemplate.Application.Features.Quotations.Commands.CreateQuotation;
using CleanArchitectureTemplate.Application.Features.Quotations.Queries.GetQuotations;
using CleanArchitectureTemplate.Application.Features.Quotations.Queries.GetQuotationById;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing quotations
/// </summary>
[ApiController]
[Route("api/dealer/quotations")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class QuotationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuotationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all quotations for the dealer
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<QuotationDto>>>> GetQuotations(
        [FromQuery] Guid? customerId)
    {
        var query = new GetQuotationsQuery { CustomerId = customerId };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<QuotationDto>>.Ok(result, "Quotations retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get quotation by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<QuotationDto>>> GetQuotationById(Guid id)
    {
        var query = new GetQuotationByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<QuotationDto>.Ok(result, "Quotation retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new quotation
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<QuotationDto>>> CreateQuotation(
        [FromBody] CreateQuotationCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<QuotationDto>.Created(result, "Quotation created successfully");
        return CreatedAtAction(nameof(GetQuotationById), new { id = result.Id }, response);
    }
}


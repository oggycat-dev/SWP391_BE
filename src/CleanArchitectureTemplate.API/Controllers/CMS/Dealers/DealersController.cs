using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.Dealers.Commands.CreateDealer;
using CleanArchitectureTemplate.Application.Features.Dealers.Commands.UpdateDealer;
using CleanArchitectureTemplate.Application.Features.Dealers.Commands.DeleteDealer;
using CleanArchitectureTemplate.Application.Features.Dealers.Queries.GetDealers;
using CleanArchitectureTemplate.Application.Features.Dealers.Queries.GetDealerById;

namespace CleanArchitectureTemplate.API.Controllers.CMS.Dealers;

/// <summary>
/// CMS API for managing dealers
/// </summary>
[ApiController]
[Route("api/cms/dealers")]
[Authorize(Roles = "Admin,EVMManager,EVMStaff")]
[ApiExplorerSettings(GroupName = "cms")]
public class DealersController : ControllerBase
{
    private readonly IMediator _mediator;

    public DealersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all dealers with pagination and filters
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<DealerDto>>>> GetDealers(
        [FromQuery] GetDealersQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<DealerDto>>.Ok(result, "Dealers retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get dealer by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DealerDto>>> GetDealerById(Guid id)
    {
        var query = new GetDealerByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<DealerDto>.Ok(result, "Dealer retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new dealer
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,EVMManager")]
    public async Task<ActionResult<ApiResponse<DealerDto>>> CreateDealer(
        [FromBody] CreateDealerCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerDto>.Created(result, "Dealer created successfully");
        return CreatedAtAction(nameof(GetDealerById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update dealer information
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,EVMManager")]
    public async Task<ActionResult<ApiResponse<DealerDto>>> UpdateDealer(
        Guid id,
        [FromBody] UpdateDealerCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<DealerDto>.BadRequest("ID mismatch"));
        }

        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerDto>.Ok(result, "Dealer updated successfully");
        return Ok(response);
    }

    /// <summary>
    /// Delete (soft delete) dealer
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDealer(Guid id)
    {
        var command = new DeleteDealerCommand { Id = id };
        var result = await _mediator.Send(command);
        var response = ApiResponse<bool>.Ok(result, "Dealer deleted successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get dealer debt summary
    /// </summary>
    [HttpGet("{id:guid}/debt")]
    public async Task<ActionResult<ApiResponse<object>>> GetDealerDebt(Guid id)
    {
        var query = new GetDealerByIdQuery { Id = id };
        var dealer = await _mediator.Send(query);
        
        var summary = new
        {
            currentDebt = dealer.CurrentDebt,
            debtLimit = dealer.DebtLimit,
            availableCredit = dealer.DebtLimit - dealer.CurrentDebt,
            overdueAmount = 0m // TODO: Calculate from actual overdue debts if needed
        };
        
        var response = ApiResponse<object>.Ok(summary, "Dealer debt retrieved successfully");
        return Ok(response);
    }
}

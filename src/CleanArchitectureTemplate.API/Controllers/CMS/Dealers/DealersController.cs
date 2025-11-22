using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.Dealers.Commands.CreateDealer;
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
}

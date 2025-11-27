using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Features.DealerDebts.Commands.CreateDealerDebt;
using CleanArchitectureTemplate.Application.Features.DealerDebts.Commands.PayDealerDebt;
using CleanArchitectureTemplate.Application.Features.DealerDebts.Queries.GetDealerDebts;
using CleanArchitectureTemplate.Application.Features.DealerDebts.Queries.GetDealerDebtById;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing dealer debts
/// </summary>
[ApiController]
[Route("api/cms/dealer-debts")]
[Authorize(Roles = "Admin,EVMManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class DealerDebtsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DealerDebtsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all dealer debts
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DealerDebtDto>>>> GetDealerDebts(
        [FromQuery] Guid? dealerId)
    {
        var query = new GetDealerDebtsQuery { DealerId = dealerId };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<DealerDebtDto>>.Ok(result, "Dealer debts retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get dealer debt by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DealerDebtDto>>> GetDealerDebtById(Guid id)
    {
        var query = new GetDealerDebtByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<DealerDebtDto>.Ok(result, "Dealer debt retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new dealer debt
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DealerDebtDto>>> CreateDealerDebt(
        [FromBody] CreateDealerDebtCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerDebtDto>.Created(result, "Dealer debt created successfully");
        return CreatedAtAction(nameof(GetDealerDebtById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Record payment for dealer debt
    /// </summary>
    [HttpPut("{id:guid}/pay")]
    public async Task<ActionResult<ApiResponse<DealerDebtDto>>> PayDealerDebt(
        Guid id,
        [FromBody] PayDealerDebtRequest request)
    {
        var command = new PayDealerDebtCommand
        {
            Id = id,
            PaymentAmount = request.PaymentAmount,
            Notes = request.Notes
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerDebtDto>.Ok(result, "Payment recorded successfully");
        return Ok(response);
    }
}


using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Sales;
using CleanArchitectureTemplate.Application.Features.SalesContracts.Commands.CreateSalesContract;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing sales contracts
/// </summary>
[ApiController]
[Route("api/dealer/contracts")]
[Authorize(Roles = "DealerManager,DealerStaff")]
[ApiExplorerSettings(GroupName = "dealer")]
public class SalesContractsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesContractsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create sales contract for an approved order
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "DealerManager")]
    public async Task<ActionResult<ApiResponse<SalesContractDto>>> CreateSalesContract(
        [FromBody] CreateSalesContractCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<SalesContractDto>.Created(result, "Sales contract created successfully");
        return Ok(response);
    }
}


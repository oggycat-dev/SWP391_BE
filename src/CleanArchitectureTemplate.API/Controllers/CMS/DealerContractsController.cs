using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.CreateDealerContract;
using CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.UpdateDealerContractStatus;
using CleanArchitectureTemplate.Application.Features.DealerContracts.Queries.GetDealerContracts;
using CleanArchitectureTemplate.Application.Features.DealerContracts.Queries.GetDealerContractById;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing dealer contracts
/// </summary>
[ApiController]
[Route("api/cms/dealer-contracts")]
[Authorize(Roles = "Admin,EVMManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class DealerContractsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DealerContractsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all dealer contracts
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DealerContractDto>>>> GetDealerContracts(
        [FromQuery] Guid? dealerId,
        [FromQuery] Domain.Enums.DealerContractStatus? status)
    {
        var query = new GetDealerContractsQuery
        {
            DealerId = dealerId,
            Status = status
        };
        
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<DealerContractDto>>.Ok(result, "Dealer contracts retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get dealer contract by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DealerContractDto>>> GetDealerContractById(Guid id)
    {
        var query = new GetDealerContractByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<DealerContractDto>.Ok(result, "Dealer contract retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new dealer contract
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DealerContractDto>>> CreateDealerContract(
        [FromBody] CreateDealerContractCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerContractDto>.Created(result, "Dealer contract created successfully");
        return CreatedAtAction(nameof(GetDealerContractById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update dealer contract status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<DealerContractDto>>> UpdateDealerContractStatus(
        Guid id,
        [FromBody] UpdateDealerContractStatusRequest request)
    {
        var command = new UpdateDealerContractStatusCommand
        {
            Id = id,
            Status = request.Status,
            SignedBy = request.SignedBy
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerContractDto>.Ok(result, "Dealer contract status updated successfully");
        return Ok(response);
    }
}


using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.CreateDealerStaff;
using CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.UpdateDealerStaff;
using CleanArchitectureTemplate.Application.Features.DealerStaff.Queries.GetDealerStaff;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing dealer staff
/// </summary>
[ApiController]
[Route("api/cms/dealer-staff")]
[Authorize(Roles = "Admin,EVMManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class DealerStaffController : ControllerBase
{
    private readonly IMediator _mediator;

    public DealerStaffController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all dealer staff
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DealerStaffDto>>>> GetDealerStaff(
        [FromQuery] Guid? dealerId)
    {
        var query = new GetDealerStaffQuery { DealerId = dealerId };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<DealerStaffDto>>.Ok(result, "Dealer staff retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Add staff member to dealer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DealerStaffDto>>> CreateDealerStaff(
        [FromBody] CreateDealerStaffCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerStaffDto>.Created(result, "Staff member added successfully");
        return Ok(response);
    }

    /// <summary>
    /// Update dealer staff information
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DealerStaffDto>>> UpdateDealerStaff(
        Guid id,
        [FromBody] UpdateDealerStaffRequest request)
    {
        var command = new UpdateDealerStaffCommand
        {
            Id = id,
            Position = request.Position,
            SalesTarget = request.SalesTarget,
            CommissionRate = request.CommissionRate,
            IsActive = request.IsActive
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerStaffDto>.Ok(result, "Staff member updated successfully");
        return Ok(response);
    }
}


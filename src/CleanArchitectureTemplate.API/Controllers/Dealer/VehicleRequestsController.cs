using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Application.Features.VehicleRequests.Commands.CreateVehicleRequest;
using CleanArchitectureTemplate.Application.Features.VehicleRequests.Queries.GetVehicleRequests;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing vehicle requests
/// </summary>
[ApiController]
[Route("api/dealer/vehicle-requests")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class VehicleRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all vehicle requests for the dealer
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VehicleRequestDto>>>> GetVehicleRequests()
    {
        var query = new GetVehicleRequestsQuery();
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<VehicleRequestDto>>.Ok(result, "Vehicle requests retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new vehicle request
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<VehicleRequestDto>>> CreateVehicleRequest(
        [FromBody] CreateVehicleRequestCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleRequestDto>.Created(result, "Vehicle request created successfully");
        return Ok(response);
    }
}


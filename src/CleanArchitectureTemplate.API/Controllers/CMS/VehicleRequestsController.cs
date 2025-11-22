using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Application.Features.VehicleRequests.Commands.ApproveVehicleRequest;
using CleanArchitectureTemplate.Application.Features.VehicleRequests.Queries.GetVehicleRequests;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing vehicle requests
/// </summary>
[ApiController]
[Route("api/cms/vehicle-requests")]
[Authorize(Roles = "Admin,EVMManager,EVMStaff")]
[ApiExplorerSettings(GroupName = "cms")]
public class VehicleRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all vehicle requests (EVM view)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<VehicleRequestDto>>>> GetVehicleRequests(
        [FromQuery] bool pendingOnly = false)
    {
        var query = new GetVehicleRequestsQuery { PendingOnly = pendingOnly };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<VehicleRequestDto>>.Ok(result, "Vehicle requests retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Approve or reject vehicle request
    /// </summary>
    [HttpPut("{id:guid}/approve")]
    [Authorize(Roles = "Admin,EVMManager")]
    public async Task<ActionResult<ApiResponse<VehicleRequestDto>>> ApproveVehicleRequest(
        Guid id,
        [FromBody] ApproveVehicleRequestRequest request)
    {
        var command = new ApproveVehicleRequestCommand
        {
            RequestId = id,
            Approved = request.Approved,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            RejectionReason = request.RejectionReason
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleRequestDto>.Ok(result, 
            request.Approved ? "Vehicle request approved successfully" : "Vehicle request rejected");
        return Ok(response);
    }
}


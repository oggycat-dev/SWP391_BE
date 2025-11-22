using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.TestDrives.Commands.CreateTestDrive;
using CleanArchitectureTemplate.Application.Features.TestDrives.Commands.UpdateTestDriveStatus;
using CleanArchitectureTemplate.Application.Features.TestDrives.Queries.GetTestDrives;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing test drives
/// </summary>
[ApiController]
[Route("api/dealer/test-drives")]
[Authorize(Roles = "DealerManager,DealerStaff")]
[ApiExplorerSettings(GroupName = "dealer")]
public class TestDrivesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestDrivesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get test drives
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<TestDriveDto>>>> GetTestDrives(
        [FromQuery] GetTestDrivesQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<TestDriveDto>>.Ok(result, "Test drives retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Schedule a new test drive
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TestDriveDto>>> CreateTestDrive(
        [FromBody] CreateTestDriveCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<TestDriveDto>.Created(result, "Test drive scheduled successfully");
        return Ok(response);
    }

    /// <summary>
    /// Update test drive status
    /// </summary>
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ApiResponse<TestDriveDto>>> UpdateTestDriveStatus(
        Guid id,
        [FromBody] UpdateTestDriveStatusCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<TestDriveDto>.BadRequest("ID mismatch"));
        }

        var result = await _mediator.Send(command);
        var response = ApiResponse<TestDriveDto>.Ok(result, "Test drive status updated successfully");
        return Ok(response);
    }
}


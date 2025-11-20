using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleModel;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleModel;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleModel;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModels;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModelById;

namespace CleanArchitectureTemplate.API.Controllers.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleModelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleModelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all vehicle models with pagination and filters
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleModelDto>>>> GetVehicleModels(
        [FromQuery] GetVehicleModelsQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleModelDto>>.Ok(result, "Vehicle models retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get vehicle model by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<VehicleModelDto>>> GetVehicleModelById(Guid id)
    {
        var query = new GetVehicleModelByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<VehicleModelDto>.Ok(result, "Vehicle model retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new vehicle model
    /// </summary>
    /// <remarks>
    /// Roles: Admin, EVMStaff
    /// </remarks>
    [HttpPost]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleModelDto>>> CreateVehicleModel(
        [FromBody] CreateVehicleModelCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleModelDto>.Created(result, "Vehicle model created successfully");
        return CreatedAtAction(nameof(GetVehicleModelById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update vehicle model
    /// </summary>
    /// <remarks>
    /// Roles: Admin, EVMStaff
    /// </remarks>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleModelDto>>> UpdateVehicleModel(
        Guid id,
        [FromBody] UpdateVehicleModelCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<VehicleModelDto>.BadRequest("ID mismatch"));
        }

        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleModelDto>.Ok(result, "Vehicle model updated successfully");
        return Ok(response);
    }

    /// <summary>
    /// Delete vehicle model
    /// </summary>
    /// <remarks>
    /// Roles: Admin
    /// </remarks>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteVehicleModel(Guid id)
    {
        var command = new DeleteVehicleModelCommand { Id = id };
        await _mediator.Send(command);
        var response = ApiResponse<bool>.NoContent("Vehicle model deleted successfully");
        return Ok(response);
    }
}

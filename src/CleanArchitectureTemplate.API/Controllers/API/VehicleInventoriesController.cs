using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleInventory;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleInventoryStatus;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicle;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventories;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventoryById;

namespace CleanArchitectureTemplate.API.Controllers.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleInventoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleInventoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,EVMStaff,EVMManager")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleInventoryDto>>>> GetVehicleInventories(
        [FromQuery] GetVehicleInventoriesQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleInventoryDto>>.Ok(result, "Vehicle inventories retrieved successfully");
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,EVMStaff,EVMManager")]
    public async Task<ActionResult<ApiResponse<VehicleInventoryDto>>> GetVehicleInventoryById(Guid id)
    {
        var query = new GetVehicleInventoryByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<VehicleInventoryDto>.Ok(result, "Vehicle inventory retrieved successfully");
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleInventoryDto>>> CreateVehicleInventory(
        [FromBody] CreateVehicleInventoryCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleInventoryDto>.Created(result, "Vehicle inventory created successfully");
        return CreatedAtAction(nameof(GetVehicleInventoryById), new { id = result.Id }, response);
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,EVMStaff,EVMManager")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateVehicleInventoryStatus(
        Guid id,
        [FromBody] UpdateVehicleInventoryStatusCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<bool>.BadRequest("ID mismatch"));
        }

        await _mediator.Send(command);
        var response = ApiResponse<bool>.Ok(true, "Vehicle inventory status updated successfully");
        return Ok(response);
    }

    [HttpPost("{id:guid}/allocate")]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<bool>>> AllocateVehicle(
        Guid id,
        [FromBody] AllocateVehicleCommand command)
    {
        if (id != command.InventoryId)
        {
            return BadRequest(ApiResponse<bool>.BadRequest("ID mismatch"));
        }

        await _mediator.Send(command);
        var response = ApiResponse<bool>.Ok(true, "Vehicle allocated successfully");
        return Ok(response);
    }
}

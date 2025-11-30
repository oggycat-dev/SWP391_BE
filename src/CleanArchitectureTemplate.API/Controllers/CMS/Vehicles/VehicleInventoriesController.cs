using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleInventory;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleInventoryStatus;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicle;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.AllocateVehicleToDealer;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventories;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventoryById;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetCentralInventory;

namespace CleanArchitectureTemplate.API.Controllers.CMS.Vehicles;

/// <summary>
/// CMS API for managing vehicle inventories
/// </summary>
[ApiController]
[Route("api/cms/vehicles/inventories")]
[Authorize(Roles = "Admin,EVMManager,EVMStaff")]
[ApiExplorerSettings(GroupName = "cms")]
public class VehicleInventoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleInventoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleInventoryDto>>>> GetVehicleInventories(
        [FromQuery] GetVehicleInventoriesQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleInventoryDto>>.Ok(result, "Vehicle inventories retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get central warehouse inventory (EVM)
    /// </summary>
    [HttpGet("central")]
    public async Task<ActionResult<ApiResponse<List<VehicleInventoryDto>>>> GetCentralInventory(
        [FromQuery] Guid? vehicleVariantId,
        [FromQuery] string? status)
    {
        var query = new GetCentralInventoryQuery
        {
            VehicleVariantId = vehicleVariantId,
            Status = status
        };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<VehicleInventoryDto>>.Ok(result, "Central inventory retrieved successfully");
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
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

    /// <summary>
    /// Allocate vehicle to an order
    /// </summary>
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

    /// <summary>
    /// Allocate vehicle to dealer (without order) - for vehicle requests
    /// </summary>
    [HttpPost("{id:guid}/allocate-to-dealer")]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<bool>>> AllocateVehicleToDealer(
        Guid id,
        [FromBody] AllocateVehicleToDealerRequest request)
    {
        if (id != request.VehicleInventoryId)
        {
            return BadRequest(ApiResponse<bool>.BadRequest("ID mismatch"));
        }

        var command = new AllocateVehicleToDealerCommand
        {
            InventoryId = request.VehicleInventoryId,
            DealerId = request.DealerId
        };

        await _mediator.Send(command);
        var response = ApiResponse<bool>.Ok(true, "Vehicle allocated to dealer successfully");
        return Ok(response);
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleColor;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleColor;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleColor;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColors;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColorById;

namespace CleanArchitectureTemplate.API.Controllers.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleColorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleColorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleColorDto>>>> GetVehicleColors(
        [FromQuery] GetVehicleColorsQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleColorDto>>.Ok(result, "Vehicle colors retrieved successfully");
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<VehicleColorDto>>> GetVehicleColorById(Guid id)
    {
        var query = new GetVehicleColorByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<VehicleColorDto>.Ok(result, "Vehicle color retrieved successfully");
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleColorDto>>> CreateVehicleColor(
        [FromBody] CreateVehicleColorCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleColorDto>.Created(result, "Vehicle color created successfully");
        return CreatedAtAction(nameof(GetVehicleColorById), new { id = result.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleColorDto>>> UpdateVehicleColor(
        Guid id,
        [FromBody] UpdateVehicleColorCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<VehicleColorDto>.BadRequest("ID mismatch"));
        }

        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleColorDto>.Ok(result, "Vehicle color updated successfully");
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteVehicleColor(Guid id)
    {
        var command = new DeleteVehicleColorCommand { Id = id };
        await _mediator.Send(command);
        var response = ApiResponse<bool>.NoContent("Vehicle color deleted successfully");
        return Ok(response);
    }
}

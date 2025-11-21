using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleVariant;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleVariant;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleVariant;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariants;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariantById;

namespace CleanArchitectureTemplate.API.Controllers.CMS.Vehicles;

/// <summary>
/// CMS API for managing vehicle variants
/// </summary>
[ApiController]
[Route("api/cms/vehicles/variants")]
[Authorize(Roles = "Admin,EVMManager,EVMStaff")]
[ApiExplorerSettings(GroupName = "cms")]
public class VehicleVariantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleVariantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleVariantDto>>>> GetVehicleVariants(
        [FromQuery] GetVehicleVariantsQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleVariantDto>>.Ok(result, "Vehicle variants retrieved successfully");
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<VehicleVariantDto>>> GetVehicleVariantById(Guid id)
    {
        var query = new GetVehicleVariantByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<VehicleVariantDto>.Ok(result, "Vehicle variant retrieved successfully");
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleVariantDto>>> CreateVehicleVariant(
        [FromBody] CreateVehicleVariantCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleVariantDto>.Created(result, "Vehicle variant created successfully");
        return CreatedAtAction(nameof(GetVehicleVariantById), new { id = result.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,EVMStaff")]
    public async Task<ActionResult<ApiResponse<VehicleVariantDto>>> UpdateVehicleVariant(
        Guid id,
        [FromBody] UpdateVehicleVariantCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(ApiResponse<VehicleVariantDto>.BadRequest("ID mismatch"));
        }

        var result = await _mediator.Send(command);
        var response = ApiResponse<VehicleVariantDto>.Ok(result, "Vehicle variant updated successfully");
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteVehicleVariant(Guid id)
    {
        var command = new DeleteVehicleVariantCommand { Id = id };
        await _mediator.Send(command);
        var response = ApiResponse<bool>.NoContent("Vehicle variant deleted successfully");
        return Ok(response);
    }
}

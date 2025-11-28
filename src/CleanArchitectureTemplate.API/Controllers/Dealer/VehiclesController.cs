using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModels;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModelById;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariants;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariantById;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColors;
using CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.CompareVehicles;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for viewing vehicle catalog
/// </summary>
[ApiController]
[Route("api/dealer/vehicles")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all vehicle models
    /// </summary>
    [HttpGet("models")]
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
    [HttpGet("models/{id:guid}")]
    public async Task<ActionResult<ApiResponse<VehicleModelDto>>> GetVehicleModelById(Guid id)
    {
        var query = new GetVehicleModelByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<VehicleModelDto>.Ok(result, "Vehicle model retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get all vehicle variants with pricing
    /// </summary>
    [HttpGet("variants")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleVariantDto>>>> GetVehicleVariants(
        [FromQuery] GetVehicleVariantsQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleVariantDto>>.Ok(result, "Vehicle variants retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get vehicle variant by ID
    /// </summary>
    [HttpGet("variants/{id:guid}")]
    public async Task<ActionResult<ApiResponse<VehicleVariantDto>>> GetVehicleVariantById(Guid id)
    {
        var query = new GetVehicleVariantByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<VehicleVariantDto>.Ok(result, "Vehicle variant retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Compare multiple vehicle variants
    /// </summary>
    [HttpGet("variants/compare")]
    public async Task<ActionResult<ApiResponse<List<VehicleVariantDto>>>> CompareVehicles(
        [FromQuery] List<Guid> variantIds)
    {
        var query = new CompareVehiclesQuery { VariantIds = variantIds };
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<VehicleVariantDto>>.Ok(result, "Vehicle comparison retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get all available vehicle colors
    /// </summary>
    [HttpGet("colors")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<VehicleColorDto>>>> GetVehicleColors(
        [FromQuery] GetVehicleColorsQuery query)
    {
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaginatedResult<VehicleColorDto>>.Ok(result, "Vehicle colors retrieved successfully");
        return Ok(response);
    }
}


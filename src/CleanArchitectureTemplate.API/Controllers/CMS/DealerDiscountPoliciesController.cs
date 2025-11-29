using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.CreateDealerDiscountPolicy;
using CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.UpdateDealerDiscountPolicy;
using CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Commands.DeleteDealerDiscountPolicy;
using CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Queries.GetDealerDiscountPolicies;
using CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Queries.GetDealerDiscountPolicyById;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for managing dealer discount policies
/// </summary>
[ApiController]
[Route("api/cms/dealer-discount-policies")]
[Authorize(Roles = "Admin,EVMManager")]
[ApiExplorerSettings(GroupName = "cms")]
public class DealerDiscountPoliciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DealerDiscountPoliciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all dealer discount policies
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DealerDiscountPolicyDto>>>> GetDealerDiscountPolicies(
        [FromQuery] Guid? dealerId,
        [FromQuery] bool? activeOnly)
    {
        var query = new GetDealerDiscountPoliciesQuery 
        { 
            DealerId = dealerId,
            ActiveOnly = activeOnly
        };
        
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<DealerDiscountPolicyDto>>.Ok(result, "Dealer discount policies retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get dealer discount policy by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DealerDiscountPolicyDto>>> GetDealerDiscountPolicyById(Guid id)
    {
        var query = new GetDealerDiscountPolicyByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<DealerDiscountPolicyDto>.Ok(result, "Dealer discount policy retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new dealer discount policy
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<DealerDiscountPolicyDto>>> CreateDealerDiscountPolicy(
        [FromBody] CreateDealerDiscountPolicyCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerDiscountPolicyDto>.Created(result, "Dealer discount policy created successfully");
        return Ok(response);
    }

    /// <summary>
    /// Update dealer discount policy
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DealerDiscountPolicyDto>>> UpdateDealerDiscountPolicy(
        Guid id,
        [FromBody] UpdateDealerDiscountPolicyRequest request)
    {
        var command = new UpdateDealerDiscountPolicyCommand
        {
            Id = id,
            DiscountRate = request.DiscountRate,
            MinOrderQuantity = request.MinOrderQuantity,
            MaxDiscountAmount = request.MaxDiscountAmount,
            EffectiveDate = request.EffectiveDate,
            ExpiryDate = request.ExpiryDate,
            IsActive = request.IsActive,
            Conditions = request.Conditions
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<DealerDiscountPolicyDto>.Ok(result, "Dealer discount policy updated successfully");
        return Ok(response);
    }

    /// <summary>
    /// Delete (deactivate) dealer discount policy
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDealerDiscountPolicy(Guid id)
    {
        var command = new DeleteDealerDiscountPolicyCommand { Id = id };
        var result = await _mediator.Send(command);
        var response = ApiResponse<bool>.Ok(result, "Dealer discount policy deleted successfully");
        return Ok(response);
    }
}


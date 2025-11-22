using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using CleanArchitectureTemplate.Application.Features.Customers.Commands.CreateCustomer;
using CleanArchitectureTemplate.Application.Features.Customers.Commands.UpdateCustomer;
using CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomers;
using CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomerById;
using CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomerHistory;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing customers
/// </summary>
[ApiController]
[Route("api/dealer/customers")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all customers for the dealer
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CustomerDto>>>> GetCustomers()
    {
        var query = new GetCustomersQuery();
        var result = await _mediator.Send(query);
        var response = ApiResponse<List<CustomerDto>>.Ok(result, "Customers retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomerById(Guid id)
    {
        var query = new GetCustomerByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<CustomerDto>.Ok(result, "Customer retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get customer history (orders, quotations, test drives)
    /// </summary>
    [HttpGet("{id:guid}/history")]
    public async Task<ActionResult<ApiResponse<CustomerHistoryDto>>> GetCustomerHistory(Guid id)
    {
        var query = new GetCustomerHistoryQuery { CustomerId = id };
        var result = await _mediator.Send(query);
        var response = ApiResponse<CustomerHistoryDto>.Ok(result, "Customer history retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create new customer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer(
        [FromBody] CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<CustomerDto>.Created(result, "Customer created successfully");
        return CreatedAtAction(nameof(GetCustomerById), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update existing customer
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(
        Guid id,
        [FromBody] UpdateCustomerRequest request)
    {
        var command = new UpdateCustomerCommand
        {
            Id = id,
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            IdNumber = request.IdNumber,
            DateOfBirth = request.DateOfBirth,
            Notes = request.Notes
        };
        
        var result = await _mediator.Send(command);
        var response = ApiResponse<CustomerDto>.Ok(result, "Customer updated successfully");
        return Ok(response);
    }
}


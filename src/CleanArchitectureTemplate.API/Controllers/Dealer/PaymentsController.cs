using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using CleanArchitectureTemplate.Application.Features.Payments.Commands.CreatePayment;
using CleanArchitectureTemplate.Application.Features.Payments.Commands.CreateInstallmentPlan;
using CleanArchitectureTemplate.Application.Features.Payments.Queries.GetPaymentsByOrder;

namespace CleanArchitectureTemplate.API.Controllers.Dealer;

/// <summary>
/// Dealer API for managing payments
/// </summary>
[ApiController]
[Route("api/dealer/payments")]
[Authorize(Roles = "DealerStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "dealer")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get payments for an order
    /// </summary>
    [HttpGet("order/{orderId:guid}")]
    public async Task<ActionResult<ApiResponse<PaymentSummaryDto>>> GetPaymentsByOrder(Guid orderId)
    {
        var query = new GetPaymentsByOrderQuery { OrderId = orderId };
        var result = await _mediator.Send(query);
        var response = ApiResponse<PaymentSummaryDto>.Ok(result, "Payments retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create payment for an order
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> CreatePayment(
        [FromBody] CreatePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<PaymentDto>.Created(result, "Payment created successfully");
        return Ok(response);
    }

    /// <summary>
    /// Create installment plan for an order
    /// </summary>
    [HttpPost("installment")]
    public async Task<ActionResult<ApiResponse<InstallmentPlanDto>>> CreateInstallmentPlan(
        [FromBody] CreateInstallmentPlanCommand command)
    {
        var result = await _mediator.Send(command);
        var response = ApiResponse<InstallmentPlanDto>.Created(result, "Installment plan created successfully");
        return Ok(response);
    }
}


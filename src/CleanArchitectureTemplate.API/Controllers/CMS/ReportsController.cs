using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using CleanArchitectureTemplate.Application.Features.Reports.Queries.GetSalesByDealer;
using CleanArchitectureTemplate.Application.Features.Reports.Queries.GetSalesByStaff;
using CleanArchitectureTemplate.Application.Features.Reports.Queries.GetInventoryTurnover;
using CleanArchitectureTemplate.Application.Features.Reports.Queries.GetDealerDebt;
using CleanArchitectureTemplate.Application.Features.Reports.Queries.GetCustomerDebt;

namespace CleanArchitectureTemplate.API.Controllers.CMS;

/// <summary>
/// CMS API for viewing reports
/// </summary>
[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin,EVMManager,EVMStaff,DealerManager")]
[ApiExplorerSettings(GroupName = "reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get sales report by dealer
    /// </summary>
    [HttpGet("sales-by-dealer")]
    public async Task<ActionResult<ApiResponse<List<SalesByDealerReport>>>> GetSalesByDealer(
        [FromQuery] DateTime? periodStart,
        [FromQuery] DateTime? periodEnd,
        [FromQuery] Guid? dealerId)
    {
        var query = new GetSalesByDealerQuery
        {
            PeriodStart = periodStart ?? DateTime.UtcNow.AddMonths(-1),
            PeriodEnd = periodEnd ?? DateTime.UtcNow,
            DealerId = dealerId
        };

        var result = await _mediator.Send(query);
        var response = ApiResponse<List<SalesByDealerReport>>.Ok(result, "Sales by dealer report retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get sales report by staff
    /// </summary>
    [HttpGet("sales-by-staff")]
    public async Task<ActionResult<ApiResponse<List<SalesByStaffReport>>>> GetSalesByStaff(
        [FromQuery] DateTime? periodStart,
        [FromQuery] DateTime? periodEnd,
        [FromQuery] Guid? dealerId,
        [FromQuery] Guid? staffId)
    {
        var query = new GetSalesByStaffQuery
        {
            PeriodStart = periodStart ?? DateTime.UtcNow.AddMonths(-1),
            PeriodEnd = periodEnd ?? DateTime.UtcNow,
            DealerId = dealerId,
            StaffId = staffId
        };

        var result = await _mediator.Send(query);
        var response = ApiResponse<List<SalesByStaffReport>>.Ok(result, "Sales by staff report retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get inventory turnover report
    /// </summary>
    [HttpGet("inventory-turnover")]
    [Authorize(Roles = "Admin,EVMManager,EVMStaff")]
    public async Task<ActionResult<ApiResponse<List<InventoryTurnoverReport>>>> GetInventoryTurnover(
        [FromQuery] Guid? vehicleVariantId)
    {
        var query = new GetInventoryTurnoverQuery
        {
            VehicleVariantId = vehicleVariantId
        };

        var result = await _mediator.Send(query);
        var response = ApiResponse<List<InventoryTurnoverReport>>.Ok(result, "Inventory turnover report retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get dealer debt report
    /// </summary>
    [HttpGet("dealer-debt")]
    [Authorize(Roles = "Admin,EVMManager")]
    public async Task<ActionResult<ApiResponse<List<DealerDebtReport>>>> GetDealerDebt(
        [FromQuery] Guid? dealerId)
    {
        var query = new GetDealerDebtQuery
        {
            DealerId = dealerId
        };

        var result = await _mediator.Send(query);
        var response = ApiResponse<List<DealerDebtReport>>.Ok(result, "Dealer debt report retrieved successfully");
        return Ok(response);
    }

    /// <summary>
    /// Get customer debt report
    /// </summary>
    [HttpGet("customer-debt")]
    public async Task<ActionResult<ApiResponse<List<CustomerDebtReport>>>> GetCustomerDebt(
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? dealerId)
    {
        var query = new GetCustomerDebtQuery
        {
            CustomerId = customerId,
            DealerId = dealerId
        };

        var result = await _mediator.Send(query);
        var response = ApiResponse<List<CustomerDebtReport>>.Ok(result, "Customer debt report retrieved successfully");
        return Ok(response);
    }
}


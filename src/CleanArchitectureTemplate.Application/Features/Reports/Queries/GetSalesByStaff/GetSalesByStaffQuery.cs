using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetSalesByStaff;

public class GetSalesByStaffQuery : IRequest<List<SalesByStaffReport>>
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public Guid? DealerId { get; set; }
    public Guid? StaffId { get; set; }
}


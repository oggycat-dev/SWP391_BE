using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetSalesByDealer;

public class GetSalesByDealerQuery : IRequest<List<SalesByDealerReport>>
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public Guid? DealerId { get; set; }
}


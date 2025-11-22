using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetDealerDebt;

public class GetDealerDebtQuery : IRequest<List<DealerDebtReport>>
{
    public Guid? DealerId { get; set; }
}


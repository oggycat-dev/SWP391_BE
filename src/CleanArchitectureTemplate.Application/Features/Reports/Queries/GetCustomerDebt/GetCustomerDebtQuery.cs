using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetCustomerDebt;

public class GetCustomerDebtQuery : IRequest<List<CustomerDebtReport>>
{
    public Guid? CustomerId { get; set; }
    public Guid? DealerId { get; set; }
}


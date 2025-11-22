using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Queries.GetQuotations;

public class GetQuotationsQuery : IRequest<List<QuotationDto>>
{
    public Guid? DealerId { get; set; }
    public Guid? CustomerId { get; set; }
}


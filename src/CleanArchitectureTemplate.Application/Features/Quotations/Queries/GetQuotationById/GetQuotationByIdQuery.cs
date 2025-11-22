using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Queries.GetQuotationById;

public class GetQuotationByIdQuery : IRequest<QuotationDto>
{
    public Guid Id { get; set; }
}


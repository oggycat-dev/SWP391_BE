using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Queries.GetDealerDebts;

public class GetDealerDebtsQuery : IRequest<List<DealerDebtDto>>
{
    public Guid? DealerId { get; set; }
}


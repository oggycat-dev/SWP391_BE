using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Queries.GetDealerDebtById;

public class GetDealerDebtByIdQuery : IRequest<DealerDebtDto>
{
    public Guid Id { get; set; }
}


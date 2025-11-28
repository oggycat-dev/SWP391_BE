using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Queries.GetDealerContractById;

public class GetDealerContractByIdQuery : IRequest<DealerContractDto>
{
    public Guid Id { get; set; }
}


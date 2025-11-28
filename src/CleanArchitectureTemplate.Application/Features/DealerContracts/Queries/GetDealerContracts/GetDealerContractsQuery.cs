using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Queries.GetDealerContracts;

public class GetDealerContractsQuery : IRequest<List<DealerContractDto>>
{
    public Guid? DealerId { get; set; }
    public DealerContractStatus? Status { get; set; }
}


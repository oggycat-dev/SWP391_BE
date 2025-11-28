using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.UpdateDealerContractStatus;

public class UpdateDealerContractStatusCommand : IRequest<DealerContractDto>
{
    public Guid Id { get; set; }
    public DealerContractStatus Status { get; set; }
    public string? SignedBy { get; set; }
}


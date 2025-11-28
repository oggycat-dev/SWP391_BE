using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.CreateDealerContract;

public class CreateDealerContractCommand : IRequest<DealerContractDto>
{
    public Guid DealerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Terms { get; set; } = string.Empty;
    public decimal CommissionRate { get; set; }
}


using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Commands.PayDealerDebt;

public class PayDealerDebtCommand : IRequest<DealerDebtDto>
{
    public Guid Id { get; set; }
    public decimal PaymentAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
}


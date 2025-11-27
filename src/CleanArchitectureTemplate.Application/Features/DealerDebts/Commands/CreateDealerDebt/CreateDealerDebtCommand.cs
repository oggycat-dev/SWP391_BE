using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Commands.CreateDealerDebt;

public class CreateDealerDebtCommand : IRequest<DealerDebtDto>
{
    public Guid DealerId { get; set; }
    public decimal TotalDebt { get; set; }
    public DateTime DueDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}


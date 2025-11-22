using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetDealerDebt;

public class GetDealerDebtQueryHandler : IRequestHandler<GetDealerDebtQuery, List<DealerDebtReport>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDealerDebtQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DealerDebtReport>> Handle(GetDealerDebtQuery request, CancellationToken cancellationToken)
    {
        var dealers = request.DealerId.HasValue
            ? new List<Domain.Entities.Dealer> { await _unitOfWork.Dealers.GetByIdAsync(request.DealerId.Value) }
            : await _unitOfWork.Dealers.GetAllAsync();

        dealers = dealers.Where(d => d != null).ToList();

        var reports = new List<DealerDebtReport>();

        foreach (var dealer in dealers)
        {
            var totalDebt = await _unitOfWork.DealerDebts.GetTotalDebtByDealerAsync(dealer.Id);
            var availableCredit = dealer.DebtLimit - totalDebt;

            var debts = await _unitOfWork.DealerDebts.GetByDealerIdAsync(dealer.Id);
            var overdueDebts = debts.Count(d => d.DueDate < DateTime.UtcNow && d.Status != DebtStatus.Paid);
            var overdueAmount = debts
                .Where(d => d.DueDate < DateTime.UtcNow && d.Status != DebtStatus.Paid)
                .Sum(d => d.TotalDebt - d.PaidAmount);

            var nextPayment = debts
                .Where(d => d.Status != DebtStatus.Paid && d.DueDate >= DateTime.UtcNow)
                .OrderBy(d => d.DueDate)
                .FirstOrDefault();

            reports.Add(new DealerDebtReport
            {
                DealerId = dealer.Id,
                DealerName = dealer.Name,
                TotalDebt = totalDebt,
                CreditLimit = dealer.DebtLimit,
                AvailableCredit = availableCredit,
                OverdueDebts = overdueDebts,
                OverdueAmount = overdueAmount,
                NextPaymentDate = nextPayment?.DueDate
            });
        }

        return reports.OrderByDescending(r => r.TotalDebt).ToList();
    }
}


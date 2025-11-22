using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetSalesByDealer;

public class GetSalesByDealerQueryHandler : IRequestHandler<GetSalesByDealerQuery, List<SalesByDealerReport>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesByDealerQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SalesByDealerReport>> Handle(GetSalesByDealerQuery request, CancellationToken cancellationToken)
    {
        var dealers = request.DealerId.HasValue
            ? new List<Domain.Entities.Dealer> { await _unitOfWork.Dealers.GetByIdAsync(request.DealerId.Value) }
            : await _unitOfWork.Dealers.GetAllAsync();

        dealers = dealers.Where(d => d != null).ToList();

        var reports = new List<SalesByDealerReport>();

        foreach (var dealer in dealers)
        {
            var orders = await _unitOfWork.Orders.GetByDealerIdAsync(dealer.Id);
            
            var periodOrders = orders
                .Where(o => o.OrderDate >= request.PeriodStart && o.OrderDate <= request.PeriodEnd)
                .ToList();

            var completedOrders = periodOrders
                .Where(o => o.Status == OrderStatus.Completed)
                .ToList();

            var totalRevenue = completedOrders.Sum(o => o.TotalAmount);
            var averageOrderValue = completedOrders.Any() ? totalRevenue / completedOrders.Count : 0;

            reports.Add(new SalesByDealerReport
            {
                DealerId = dealer.Id,
                DealerName = dealer.Name,
                TotalOrders = periodOrders.Count,
                CompletedOrders = completedOrders.Count,
                TotalRevenue = totalRevenue,
                AverageOrderValue = averageOrderValue,
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd
            });
        }

        return reports.OrderByDescending(r => r.TotalRevenue).ToList();
    }
}


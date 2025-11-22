using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetCustomerDebt;

public class GetCustomerDebtQueryHandler : IRequestHandler<GetCustomerDebtQuery, List<CustomerDebtReport>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerDebtQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CustomerDebtReport>> Handle(GetCustomerDebtQuery request, CancellationToken cancellationToken)
    {
        var orders = new List<Domain.Entities.Order>();

        if (request.CustomerId.HasValue)
        {
            orders = await _unitOfWork.Orders.GetByCustomerIdAsync(request.CustomerId.Value);
        }
        else if (request.DealerId.HasValue)
        {
            orders = await _unitOfWork.Orders.GetByDealerIdAsync(request.DealerId.Value);
        }
        else
        {
            orders = await _unitOfWork.Orders.GetAllAsync();
        }

        // Only orders with remaining amount (installment or partial payment)
        var ordersWithDebt = orders.Where(o => o.RemainingAmount > 0).ToList();

        var reports = new List<CustomerDebtReport>();

        foreach (var order in ordersWithDebt)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId);
            var installmentPlan = await _unitOfWork.InstallmentPlans.GetByOrderIdAsync(order.Id);

            var isOverdue = installmentPlan != null && installmentPlan.NextPaymentDate < DateTime.UtcNow;

            reports.Add(new CustomerDebtReport
            {
                CustomerId = order.CustomerId,
                CustomerName = customer?.FullName ?? "Unknown",
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                PaidAmount = order.PaidAmount,
                RemainingAmount = order.RemainingAmount,
                NextPaymentDate = installmentPlan?.NextPaymentDate,
                IsOverdue = isOverdue
            });
        }

        return reports.OrderByDescending(r => r.RemainingAmount).ToList();
    }
}


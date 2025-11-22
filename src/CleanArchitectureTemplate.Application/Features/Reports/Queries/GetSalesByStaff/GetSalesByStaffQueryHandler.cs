using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetSalesByStaff;

public class GetSalesByStaffQueryHandler : IRequestHandler<GetSalesByStaffQuery, List<SalesByStaffReport>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesByStaffQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SalesByStaffReport>> Handle(GetSalesByStaffQuery request, CancellationToken cancellationToken)
    {
        var dealerStaffs = new List<Domain.Entities.DealerStaff>();

        if (request.StaffId.HasValue)
        {
            var staff = await _unitOfWork.DealerStaff.GetByIdWithDetailsAsync(request.StaffId.Value);
            if (staff != null) dealerStaffs.Add(staff);
        }
        else if (request.DealerId.HasValue)
        {
            dealerStaffs = await _unitOfWork.DealerStaff.GetByDealerIdAsync(request.DealerId.Value);
        }
        else
        {
            dealerStaffs = await _unitOfWork.DealerStaff.GetAllAsync();
        }

        var reports = new List<SalesByStaffReport>();

        foreach (var staff in dealerStaffs)
        {
            var orders = await _unitOfWork.Orders.GetByDealerStaffIdAsync(staff.Id);
            
            var periodOrders = orders
                .Where(o => o.OrderDate >= request.PeriodStart && o.OrderDate <= request.PeriodEnd)
                .ToList();

            var completedOrders = periodOrders
                .Where(o => o.Status == OrderStatus.Completed)
                .ToList();

            var totalRevenue = completedOrders.Sum(o => o.TotalAmount);
            var commission = totalRevenue * (staff.CommissionRate / 100);

            var dealer = await _unitOfWork.Dealers.GetByIdAsync(staff.DealerId);
            var user = await _unitOfWork.Users.GetByIdAsync(staff.UserId);

            reports.Add(new SalesByStaffReport
            {
                StaffId = staff.Id,
                StaffName = user?.FullName ?? "Unknown",
                DealerId = staff.DealerId,
                DealerName = dealer?.Name ?? "Unknown",
                TotalOrders = periodOrders.Count,
                CompletedOrders = completedOrders.Count,
                TotalRevenue = totalRevenue,
                Commission = commission,
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd
            });
        }

        return reports.OrderByDescending(r => r.TotalRevenue).ToList();
    }
}


using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetInventoryTurnover;

public class GetInventoryTurnoverQueryHandler : IRequestHandler<GetInventoryTurnoverQuery, List<InventoryTurnoverReport>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInventoryTurnoverQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<InventoryTurnoverReport>> Handle(GetInventoryTurnoverQuery request, CancellationToken cancellationToken)
    {
        var variants = request.VehicleVariantId.HasValue
            ? new List<Domain.Entities.VehicleVariant> { await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(request.VehicleVariantId.Value) }
            : await _unitOfWork.VehicleVariants.GetAllWithModelsAsync();

        variants = variants.Where(v => v != null).ToList();

        var reports = new List<InventoryTurnoverReport>();

        foreach (var variant in variants)
        {
            var inventories = await _unitOfWork.VehicleInventories.GetAllAsync();
            var variantInventories = inventories.Where(i => i.VariantId == variant.Id).ToList();

            var totalAvailable = variantInventories.Count(i => i.Status == VehicleStatus.Available);
            var totalReserved = variantInventories.Count(i => i.Status == VehicleStatus.Reserved);
            var totalSold = variantInventories.Count(i => i.Status == VehicleStatus.Sold);
            var totalInTransit = variantInventories.Count(i => i.Status == VehicleStatus.InTransit);

            // Calculate average days in inventory for sold vehicles (from created date to sold date)
            var soldVehicles = variantInventories.Where(i => i.Status == VehicleStatus.Sold && i.SoldDate.HasValue).ToList();
            var avgDaysInInventory = soldVehicles.Any()
                ? (int)soldVehicles.Average(v => (v.SoldDate!.Value - v.CreatedAt).TotalDays)
                : 0;

            // Calculate turnover rate (sold / total available in last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var recentSold = variantInventories.Count(i => i.Status == VehicleStatus.Sold && i.SoldDate >= thirtyDaysAgo);
            var turnoverRate = totalAvailable > 0 ? (decimal)recentSold / totalAvailable * 100 : 0;

            reports.Add(new InventoryTurnoverReport
            {
                VehicleVariantId = variant.Id,
                VehicleVariantName = variant.VariantName,
                VehicleModelName = variant.Model.ModelName,
                TotalAvailable = totalAvailable,
                TotalReserved = totalReserved,
                TotalSold = totalSold,
                TotalInTransit = totalInTransit,
                DaysInInventory = avgDaysInInventory,
                TurnoverRate = turnoverRate
            });
        }

        return reports.OrderByDescending(r => r.TurnoverRate).ToList();
    }
}


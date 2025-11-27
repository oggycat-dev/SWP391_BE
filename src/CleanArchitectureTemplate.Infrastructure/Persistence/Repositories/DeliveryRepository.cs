using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
{
    public DeliveryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Delivery?> GetByDeliveryCodeAsync(string deliveryCode)
    {
        return await _dbSet
            .Include(d => d.Order)
                .ThenInclude(o => o.Customer)
            .Include(d => d.Order)
                .ThenInclude(o => o.Dealer)
            .FirstOrDefaultAsync(d => d.DeliveryCode == deliveryCode);
    }

    public async Task<Delivery?> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbSet
            .Include(d => d.Order)
            .FirstOrDefaultAsync(d => d.OrderId == orderId);
    }

    public async Task<List<Delivery>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Include(d => d.Order)
                .ThenInclude(o => o.Customer)
            .Where(d => d.Order.DealerId == dealerId)
            .OrderByDescending(d => d.ScheduledDate)
            .ToListAsync();
    }

    public async Task<Delivery?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(d => d.Order)
                .ThenInclude(o => o.Customer)
            .Include(d => d.Order)
                .ThenInclude(o => o.Dealer)
            .Include(d => d.Order)
                .ThenInclude(o => o.VehicleVariant)
                    .ThenInclude(v => v.Model)
            .Include(d => d.Order)
                .ThenInclude(o => o.VehicleColor)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}


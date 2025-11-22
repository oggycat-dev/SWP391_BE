using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }

    public async Task<List<Order>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(o => o.DealerId == dealerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<List<Order>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.Dealer)
            .Include(o => o.DealerStaff)
                .ThenInclude(ds => ds.User)
            .Include(o => o.VehicleVariant)
                .ThenInclude(v => v.Model)
            .Include(o => o.VehicleColor)
            .Include(o => o.VehicleInventory)
            .Include(o => o.Payments)
            .Include(o => o.Promotions)
                .ThenInclude(op => op.Promotion)
            .Include(o => o.SalesContract)
            .Include(o => o.Delivery)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetByDealerStaffIdAsync(Guid dealerStaffId)
    {
        return await _dbSet
            .Where(o => o.DealerStaffId == dealerStaffId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
}


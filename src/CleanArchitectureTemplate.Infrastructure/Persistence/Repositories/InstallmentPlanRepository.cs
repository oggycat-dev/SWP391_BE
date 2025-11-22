using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class InstallmentPlanRepository : Repository<InstallmentPlan>, IInstallmentPlanRepository
{
    public InstallmentPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<InstallmentPlan?> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbSet.FirstOrDefaultAsync(ip => ip.OrderId == orderId);
    }

    public async Task<List<InstallmentPlan>> GetOverdueInstallmentsAsync()
    {
        return await _dbSet
            .Where(ip => ip.NextPaymentDate < DateTime.UtcNow && ip.RemainingAmount > 0)
            .Include(ip => ip.Order)
                .ThenInclude(o => o.Customer)
            .ToListAsync();
    }
}


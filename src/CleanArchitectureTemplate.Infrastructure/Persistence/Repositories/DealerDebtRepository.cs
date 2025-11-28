using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DealerDebtRepository : Repository<DealerDebt>, IDealerDebtRepository
{
    public DealerDebtRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<DealerDebt>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(dd => dd.DealerId == dealerId)
            .OrderByDescending(dd => dd.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DealerDebt>> GetOverdueDebtsAsync()
    {
        return await _dbSet
            .Where(dd => dd.Status == DebtStatus.Overdue || 
                        (dd.DueDate < DateTime.UtcNow && dd.RemainingAmount > 0))
            .OrderBy(dd => dd.DueDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalDebtByDealerAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(dd => dd.DealerId == dealerId)
            .SumAsync(dd => dd.RemainingAmount);
    }
}

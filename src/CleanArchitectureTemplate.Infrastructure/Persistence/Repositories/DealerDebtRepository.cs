using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

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
            .Include(dd => dd.Dealer)
            .OrderByDescending(dd => dd.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<DealerDebt>> GetOverdueDebtsAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(dd => dd.Status == DebtStatus.Current 
                && dd.DueDate < today)
            .Include(dd => dd.Dealer)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalDebtByDealerAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(dd => dd.DealerId == dealerId 
                && (dd.Status == DebtStatus.Current || dd.Status == DebtStatus.Overdue))
            .SumAsync(dd => dd.TotalDebt - dd.PaidAmount);
    }
}

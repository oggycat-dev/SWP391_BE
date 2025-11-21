using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DealerRepository : Repository<Dealer>, IDealerRepository
{
    public DealerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Dealer?> GetByDealerCodeAsync(string dealerCode)
    {
        return await _dbSet
            .Include(d => d.Contracts)
            .Include(d => d.Staff)
            .FirstOrDefaultAsync(d => d.DealerCode == dealerCode);
    }

    public async Task<List<Dealer>> GetActiveDealersAsync()
    {
        return await _dbSet
            .Where(d => d.Status == DealerStatus.Active)
            .ToListAsync();
    }

    public async Task<Dealer?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(d => d.Contracts)
            .Include(d => d.Staff)
            .Include(d => d.Debts)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}

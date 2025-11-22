using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DealerContractRepository : Repository<DealerContract>, IDealerContractRepository
{
    public DealerContractRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<DealerContract?> GetByContractNumberAsync(string contractNumber)
    {
        return await _dbSet
            .Include(dc => dc.Dealer)
            .FirstOrDefaultAsync(dc => dc.ContractNumber == contractNumber);
    }

    public async Task<List<DealerContract>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(dc => dc.DealerId == dealerId)
            .OrderByDescending(dc => dc.StartDate)
            .ToListAsync();
    }

    public async Task<DealerContract?> GetActiveDealerContractAsync(Guid dealerId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(dc => dc.DealerId == dealerId 
                && dc.StartDate <= today 
                && dc.EndDate >= today)
            .FirstOrDefaultAsync();
    }
}

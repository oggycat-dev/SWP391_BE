using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DealerContractRepository : Repository<DealerContract>, IDealerContractRepository
{
    public DealerContractRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<DealerContract?> GetByContractNumberAsync(string contractNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(dc => dc.ContractNumber == contractNumber);
    }

    public async Task<List<DealerContract>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(dc => dc.DealerId == dealerId)
            .OrderByDescending(dc => dc.CreatedAt)
            .ToListAsync();
    }

    public async Task<DealerContract?> GetActiveDealerContractAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(dc => dc.DealerId == dealerId && 
                        dc.Status == DealerContractStatus.Active &&
                        dc.StartDate <= DateTime.UtcNow &&
                        dc.EndDate >= DateTime.UtcNow)
            .OrderByDescending(dc => dc.StartDate)
            .FirstOrDefaultAsync();
    }
}

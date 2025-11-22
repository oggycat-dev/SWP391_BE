using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class PromotionRepository : Repository<Promotion>, IPromotionRepository
{
    public PromotionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Promotion?> GetByPromotionCodeAsync(string promotionCode)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.PromotionCode == promotionCode);
    }

    public async Task<List<Promotion>> GetActivePromotionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(p => p.Status == PromotionStatus.Active 
                && p.StartDate <= now 
                && p.EndDate >= now)
            .ToListAsync();
    }
}


using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DealerDiscountPolicyRepository : Repository<DealerDiscountPolicy>, IDealerDiscountPolicyRepository
{
    public DealerDiscountPolicyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<DealerDiscountPolicy>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Include(ddp => ddp.VehicleVariant)
            .Where(ddp => ddp.DealerId == dealerId)
            .OrderByDescending(ddp => ddp.IsActive)
            .ThenByDescending(ddp => ddp.EffectiveDate)
            .ToListAsync();
    }

    public async Task<List<DealerDiscountPolicy>> GetActivePolicesAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Include(ddp => ddp.VehicleVariant)
            .Include(ddp => ddp.Dealer)
            .Where(ddp => ddp.IsActive && 
                         ddp.EffectiveDate <= now &&
                         (ddp.ExpiryDate == null || ddp.ExpiryDate >= now))
            .ToListAsync();
    }

    public async Task<List<DealerDiscountPolicy>> GetByVehicleVariantIdAsync(Guid vehicleVariantId)
    {
        return await _dbSet
            .Include(ddp => ddp.Dealer)
            .Where(ddp => ddp.VehicleVariantId == vehicleVariantId && ddp.IsActive)
            .ToListAsync();
    }
}


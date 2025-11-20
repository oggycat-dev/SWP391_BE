using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class VehicleColorRepository : Repository<VehicleColor>, IVehicleColorRepository
{
    public VehicleColorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<VehicleColor>> GetByVariantIdAsync(Guid variantId)
    {
        return await _dbSet
            .Where(vc => vc.VariantId == variantId && vc.IsActive)
            .Include(vc => vc.Variant)
            .ToListAsync();
    }

    public async Task<VehicleColor?> GetByColorCodeAsync(string colorCode)
    {
        return await _dbSet
            .Include(vc => vc.Variant)
            .FirstOrDefaultAsync(vc => vc.ColorCode == colorCode);
    }

    public async Task<VehicleColor?> GetByIdWithVariantAsync(Guid id)
    {
        return await _dbSet
            .Include(vc => vc.Variant)
            .FirstOrDefaultAsync(vc => vc.Id == id);
    }

    public async Task<List<VehicleColor>> GetAllWithVariantsAsync()
    {
        return await _dbSet
            .Include(vc => vc.Variant)
            .ToListAsync();
    }
}

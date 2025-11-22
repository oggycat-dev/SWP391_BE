using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class VehicleVariantRepository : Repository<VehicleVariant>, IVehicleVariantRepository
{
    public VehicleVariantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<VehicleVariant?> GetByVariantCodeAsync(string variantCode)
    {
        return await _dbSet
            .Include(vv => vv.Model)
            .Include(vv => vv.Colors)
            .FirstOrDefaultAsync(vv => vv.VariantCode == variantCode);
    }

    public async Task<IEnumerable<VehicleVariant>> GetByModelIdAsync(Guid modelId)
    {
        return await _dbSet
            .Where(vv => vv.ModelId == modelId)
            .Include(vv => vv.Colors)
            .ToListAsync();
    }

    public async Task<VehicleVariant?> GetByIdWithModelAsync(Guid id)
    {
        return await _dbSet
            .Include(vv => vv.Model)
            .FirstOrDefaultAsync(vv => vv.Id == id);
    }

    public async Task<List<VehicleVariant>> GetAllWithModelsAsync()
    {
        return await _dbSet
            .Include(vv => vv.Model)
            .ToListAsync();
    }
}

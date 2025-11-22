using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class VehicleModelRepository : Repository<VehicleModel>, IVehicleModelRepository
{
    public VehicleModelRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<VehicleModel?> GetByModelCodeAsync(string modelCode)
    {
        return await _dbSet
            .Include(vm => vm.Variants)
            .FirstOrDefaultAsync(vm => vm.ModelCode == modelCode);
    }

    public async Task<IEnumerable<VehicleModel>> GetActiveModelsAsync()
    {
        return await _dbSet
            .Where(vm => vm.IsActive)
            .Include(vm => vm.Variants)
            .ToListAsync();
    }
}

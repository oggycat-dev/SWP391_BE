using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class VehicleInventoryRepository : Repository<VehicleInventory>, IVehicleInventoryRepository
{
    public VehicleInventoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<VehicleInventory?> GetByVINAsync(string vinNumber)
    {
        return await _dbSet
            .Include(vi => vi.Variant)
                .ThenInclude(vv => vv.Model)
            .Include(vi => vi.Color)
            .Include(vi => vi.Dealer)
            .FirstOrDefaultAsync(vi => vi.VINNumber == vinNumber);
    }

    public async Task<IEnumerable<VehicleInventory>> GetAvailableVehiclesAsync()
    {
        return await _dbSet
            .Where(vi => vi.Status == VehicleStatus.Available)
            .Include(vi => vi.Variant)
                .ThenInclude(vv => vv.Model)
            .Include(vi => vi.Color)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleInventory>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(vi => vi.DealerId == dealerId)
            .Include(vi => vi.Variant)
                .ThenInclude(vv => vv.Model)
            .Include(vi => vi.Color)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleInventory>> GetByStatusAsync(string status)
    {
        if (!Enum.TryParse<VehicleStatus>(status, out var vehicleStatus))
        {
            return Enumerable.Empty<VehicleInventory>();
        }

        return await _dbSet
            .Where(vi => vi.Status == vehicleStatus)
            .Include(vi => vi.Variant)
                .ThenInclude(vv => vv.Model)
            .Include(vi => vi.Color)
            .Include(vi => vi.Dealer)
            .ToListAsync();
    }

    public async Task<VehicleInventory?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(vi => vi.Variant)
                .ThenInclude(vv => vv.Model)
            .Include(vi => vi.Color)
            .Include(vi => vi.Dealer)
            .FirstOrDefaultAsync(vi => vi.Id == id);
    }

    public async Task<List<VehicleInventory>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(vi => vi.Variant)
                .ThenInclude(vv => vv.Model)
            .Include(vi => vi.Color)
            .ToListAsync();
    }
}

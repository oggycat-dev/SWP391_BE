using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class VehicleRequestRepository : Repository<VehicleRequest>, IVehicleRequestRepository
{
    public VehicleRequestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<VehicleRequest?> GetByRequestCodeAsync(string requestCode)
    {
        return await _dbSet.FirstOrDefaultAsync(vr => vr.RequestCode == requestCode);
    }

    public async Task<List<VehicleRequest>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(vr => vr.DealerId == dealerId)
            .Include(vr => vr.VehicleVariant)
                .ThenInclude(v => v.Model)
            .Include(vr => vr.VehicleColor)
            .OrderByDescending(vr => vr.RequestDate)
            .ToListAsync();
    }

    public async Task<List<VehicleRequest>> GetPendingRequestsAsync()
    {
        return await _dbSet
            .Where(vr => vr.Status == VehicleRequestStatus.Pending)
            .Include(vr => vr.Dealer)
            .Include(vr => vr.VehicleVariant)
                .ThenInclude(v => v.Model)
            .Include(vr => vr.VehicleColor)
            .OrderBy(vr => vr.RequestDate)
            .ToListAsync();
    }
}


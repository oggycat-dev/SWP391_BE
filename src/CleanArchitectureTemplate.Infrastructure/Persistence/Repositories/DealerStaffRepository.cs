using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class DealerStaffRepository : Repository<DealerStaff>, IDealerStaffRepository
{
    public DealerStaffRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<DealerStaff>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Include(ds => ds.User)
            .Include(ds => ds.Dealer)
            .Where(ds => ds.DealerId == dealerId)
            .OrderByDescending(ds => ds.IsActive)
            .ThenBy(ds => ds.User.FirstName)
            .ToListAsync();
    }

    public async Task<DealerStaff?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(ds => ds.User)
            .Include(ds => ds.Dealer)
            .FirstOrDefaultAsync(ds => ds.UserId == userId && ds.IsActive);
    }

    public async Task<DealerStaff?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(ds => ds.User)
            .Include(ds => ds.Dealer)
            .FirstOrDefaultAsync(ds => ds.Id == id);
    }
}

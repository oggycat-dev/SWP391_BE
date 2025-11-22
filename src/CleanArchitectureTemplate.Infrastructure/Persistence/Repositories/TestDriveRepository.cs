using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class TestDriveRepository : Repository<TestDrive>, ITestDriveRepository
{
    public TestDriveRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<TestDrive>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(td => td.DealerId == dealerId)
            .Include(td => td.Customer)
            .Include(td => td.VehicleVariant)
                .ThenInclude(v => v.Model)
            .Include(td => td.AssignedStaff)
                .ThenInclude(ds => ds.User)
            .OrderByDescending(td => td.ScheduledDate)
            .ToListAsync();
    }

    public async Task<List<TestDrive>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Where(td => td.CustomerId == customerId)
            .Include(td => td.VehicleVariant)
                .ThenInclude(v => v.Model)
            .OrderByDescending(td => td.ScheduledDate)
            .ToListAsync();
    }

    public async Task<List<TestDrive>> GetUpcomingTestDrivesAsync(Guid dealerId)
    {
        var today = DateTime.UtcNow.Date;
        return await _dbSet
            .Where(td => td.DealerId == dealerId && td.ScheduledDate >= today)
            .Include(td => td.Customer)
            .Include(td => td.VehicleVariant)
                .ThenInclude(v => v.Model)
            .Include(td => td.AssignedStaff)
                .ThenInclude(ds => ds.User)
            .OrderBy(td => td.ScheduledDate)
            .ToListAsync();
    }
}


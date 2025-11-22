using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByCustomerCodeAsync(string customerCode)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.CustomerCode == customerCode);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
    }

    public async Task<List<Customer>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet.Where(c => c.CreatedByDealerId == dealerId).ToListAsync();
    }

    public async Task<Customer?> GetByIdWithOrdersAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.Orders)
            .Include(c => c.TestDrives)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}


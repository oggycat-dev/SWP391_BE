using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class QuotationRepository : Repository<Quotation>, IQuotationRepository
{
    public QuotationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Quotation?> GetByQuotationNumberAsync(string quotationNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(q => q.QuotationNumber == quotationNumber);
    }

    public async Task<List<Quotation>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Where(q => q.DealerId == dealerId)
            .OrderByDescending(q => q.QuotationDate)
            .ToListAsync();
    }

    public async Task<List<Quotation>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Where(q => q.CustomerId == customerId)
            .OrderByDescending(q => q.QuotationDate)
            .ToListAsync();
    }

    public async Task<Quotation?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(q => q.Customer)
            .Include(q => q.Dealer)
            .Include(q => q.DealerStaff)
                .ThenInclude(ds => ds.User)
            .Include(q => q.VehicleVariant)
                .ThenInclude(v => v.Model)
            .Include(q => q.VehicleColor)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}


using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class SalesContractRepository : Repository<SalesContract>, ISalesContractRepository
{
    public SalesContractRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SalesContract?> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbSet
            .Include(sc => sc.Order)
                .ThenInclude(o => o.Customer)
            .FirstOrDefaultAsync(sc => sc.OrderId == orderId);
    }

    public async Task<SalesContract?> GetByContractNumberAsync(string contractNumber)
    {
        return await _dbSet
            .Include(sc => sc.Order)
                .ThenInclude(o => o.Customer)
            .FirstOrDefaultAsync(sc => sc.ContractNumber == contractNumber);
    }
}


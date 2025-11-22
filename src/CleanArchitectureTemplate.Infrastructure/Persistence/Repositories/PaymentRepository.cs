using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Payment>> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbSet
            .Where(p => p.OrderId == orderId)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalPaidByOrderIdAsync(Guid orderId)
    {
        return await _dbSet
            .Where(p => p.OrderId == orderId && p.Status == PaymentStatus.Completed)
            .SumAsync(p => p.Amount);
    }
}


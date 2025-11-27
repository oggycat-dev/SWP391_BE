using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

public class CustomerFeedbackRepository : Repository<CustomerFeedback>, ICustomerFeedbackRepository
{
    public CustomerFeedbackRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<CustomerFeedback>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbSet
            .Include(f => f.Customer)
            .Include(f => f.Dealer)
            .Include(f => f.Order)
            .Include(f => f.Responder)
            .Where(f => f.CustomerId == customerId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<CustomerFeedback>> GetByDealerIdAsync(Guid dealerId)
    {
        return await _dbSet
            .Include(f => f.Customer)
            .Include(f => f.Dealer)
            .Include(f => f.Order)
            .Include(f => f.Responder)
            .Where(f => f.DealerId == dealerId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<CustomerFeedback>> GetByOrderIdAsync(Guid orderId)
    {
        return await _dbSet
            .Include(f => f.Customer)
            .Include(f => f.Dealer)
            .Include(f => f.Order)
            .Include(f => f.Responder)
            .Where(f => f.OrderId == orderId)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<CustomerFeedback>> GetByStatusAsync(FeedbackStatus status)
    {
        return await _dbSet
            .Include(f => f.Customer)
            .Include(f => f.Dealer)
            .Include(f => f.Order)
            .Include(f => f.Responder)
            .Where(f => f.FeedbackStatus == status)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<CustomerFeedback?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(f => f.Customer)
            .Include(f => f.Dealer)
            .Include(f => f.Order)
            .Include(f => f.Responder)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}


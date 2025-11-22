using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomerHistory;

public class GetCustomerHistoryQueryHandler : IRequestHandler<GetCustomerHistoryQuery, CustomerHistoryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerHistoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerHistoryDto> Handle(GetCustomerHistoryQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdWithOrdersAsync(request.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException("Customer", request.CustomerId);
        }

        var orders = await _unitOfWork.Orders.GetByCustomerIdAsync(request.CustomerId);
        var quotations = await _unitOfWork.Quotations.GetByCustomerIdAsync(request.CustomerId);
        var testDrives = await _unitOfWork.TestDrives.GetByCustomerIdAsync(request.CustomerId);

        var orderHistory = new List<OrderHistoryDto>();
        foreach (var order in orders)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(order.VehicleVariantId);
            orderHistory.Add(new OrderHistoryDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                VehicleName = variant != null ? $"{variant.Model.ModelName} {variant.VariantName}" : "Unknown",
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                PaidAmount = order.PaidAmount
            });
        }

        var quotationHistory = new List<QuotationHistoryDto>();
        foreach (var quotation in quotations)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(quotation.VehicleVariantId);
            quotationHistory.Add(new QuotationHistoryDto
            {
                Id = quotation.Id,
                QuotationNumber = quotation.QuotationNumber,
                QuotationDate = quotation.QuotationDate,
                VehicleName = variant != null ? $"{variant.Model.ModelName} {variant.VariantName}" : "Unknown",
                Status = quotation.Status.ToString(),
                TotalAmount = quotation.TotalAmount
            });
        }

        var testDriveHistory = new List<TestDriveHistoryDto>();
        foreach (var testDrive in testDrives)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(testDrive.VehicleVariantId);
            testDriveHistory.Add(new TestDriveHistoryDto
            {
                Id = testDrive.Id,
                ScheduledDate = testDrive.ScheduledDate,
                VehicleName = variant != null ? $"{variant.Model.ModelName} {variant.VariantName}" : "Unknown",
                Status = testDrive.Status.ToString(),
                Feedback = testDrive.Feedback
            });
        }

        var totalSpent = orders.Where(o => o.Status == OrderStatus.Completed).Sum(o => o.PaidAmount);
        var completedOrders = orders.Count(o => o.Status == OrderStatus.Completed);

        return new CustomerHistoryDto
        {
            CustomerId = customer.Id,
            CustomerName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            CreatedAt = customer.CreatedAt,
            Orders = orderHistory.OrderByDescending(o => o.OrderDate).ToList(),
            Quotations = quotationHistory.OrderByDescending(q => q.QuotationDate).ToList(),
            TestDrives = testDriveHistory.OrderByDescending(t => t.ScheduledDate).ToList(),
            TotalSpent = totalSpent,
            TotalOrders = orders.Count,
            CompletedOrders = completedOrders
        };
    }
}


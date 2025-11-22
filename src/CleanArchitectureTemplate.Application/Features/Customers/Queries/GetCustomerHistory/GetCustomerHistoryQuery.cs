using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomerHistory;

public class GetCustomerHistoryQuery : IRequest<CustomerHistoryDto>
{
    public Guid CustomerId { get; set; }
}

public class CustomerHistoryDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public List<OrderHistoryDto> Orders { get; set; } = new();
    public List<TestDriveHistoryDto> TestDrives { get; set; } = new();
    public List<QuotationHistoryDto> Quotations { get; set; } = new();
    
    public decimal TotalSpent { get; set; }
    public int TotalOrders { get; set; }
    public int CompletedOrders { get; set; }
}

public class OrderHistoryDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
}

public class TestDriveHistoryDto
{
    public Guid Id { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Feedback { get; set; }
}

public class QuotationHistoryDto
{
    public Guid Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}


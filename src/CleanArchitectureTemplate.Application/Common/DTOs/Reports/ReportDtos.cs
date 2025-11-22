namespace CleanArchitectureTemplate.Application.Common.DTOs.Reports;

public class SalesByDealerReport
{
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public int CompletedOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class SalesByStaffReport
{
    public Guid StaffId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public int CompletedOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal Commission { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class InventoryTurnoverReport
{
    public Guid VehicleVariantId { get; set; }
    public string VehicleVariantName { get; set; } = string.Empty;
    public string VehicleModelName { get; set; } = string.Empty;
    public int TotalAvailable { get; set; }
    public int TotalReserved { get; set; }
    public int TotalSold { get; set; }
    public int TotalInTransit { get; set; }
    public int DaysInInventory { get; set; }
    public decimal TurnoverRate { get; set; }
}

public class DealerDebtReport
{
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public decimal TotalDebt { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal AvailableCredit { get; set; }
    public int OverdueDebts { get; set; }
    public decimal OverdueAmount { get; set; }
    public DateTime? NextPaymentDate { get; set; }
}

public class CustomerDebtReport
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    public bool IsOverdue { get; set; }
}


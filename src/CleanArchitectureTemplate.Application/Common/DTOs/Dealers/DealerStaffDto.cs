namespace CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

public class DealerStaffDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal SalesTarget { get; set; }
    public decimal CurrentSales { get; set; }
    public decimal CommissionRate { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime? LeftDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDealerStaffRequest
{
    public Guid UserId { get; set; }
    public Guid DealerId { get; set; }
    public string Position { get; set; } = string.Empty;
    public decimal SalesTarget { get; set; }
    public decimal CommissionRate { get; set; }
    public DateTime JoinedDate { get; set; }
}

public class UpdateDealerStaffRequest
{
    public string Position { get; set; } = string.Empty;
    public decimal SalesTarget { get; set; }
    public decimal CommissionRate { get; set; }
    public bool IsActive { get; set; }
}

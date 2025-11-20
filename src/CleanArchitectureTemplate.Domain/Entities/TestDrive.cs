using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Lịch hẹn lái thử
/// </summary>
public class TestDrive : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid DealerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    
    public DateTime ScheduledDate { get; set; }
    public TestDriveStatus Status { get; set; } = TestDriveStatus.Scheduled;
    
    public string Notes { get; set; } = string.Empty;
    public string Feedback { get; set; } = string.Empty;
    public int? Rating { get; set; } // 1-5
    
    public Guid? AssignedStaffId { get; set; } // Nhân viên phụ trách
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Dealer Dealer { get; set; } = null!;
    public VehicleVariant VehicleVariant { get; set; } = null!;
    public DealerStaff? AssignedStaff { get; set; }
}

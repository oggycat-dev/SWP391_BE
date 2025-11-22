using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;

public class TestDriveDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid VehicleVariantId { get; set; }
    public string VehicleVariantName { get; set; } = string.Empty;
    public string VehicleModelName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public TestDriveStatus Status { get; set; }
    public Guid? AssignedStaffId { get; set; }
    public string? AssignedStaffName { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string? Feedback { get; set; }
    public int? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTestDriveRequest
{
    public Guid CustomerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class UpdateTestDriveStatusRequest
{
    public TestDriveStatus Status { get; set; }
    public Guid? AssignedStaffId { get; set; }
    public string? Feedback { get; set; }
}


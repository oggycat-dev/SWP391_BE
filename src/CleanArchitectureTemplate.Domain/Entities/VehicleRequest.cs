using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Yêu cầu đặt xe từ đại lý
/// </summary>
public class VehicleRequest : BaseEntity
{
    public Guid DealerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    
    public string RequestCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    
    public DateTime RequestDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    
    public VehicleRequestStatus Status { get; set; } = VehicleRequestStatus.Pending;
    
    public Guid? ApprovedBy { get; set; }
    public string RequestReason { get; set; } = string.Empty;
    public string ApprovalNotes { get; set; } = string.Empty;
    public string RejectionReason { get; set; } = string.Empty;
    
    public string Notes { get; set; } = string.Empty;
    public string RequestNotes { get; set; } = string.Empty;
    
    // Navigation properties
    public Dealer Dealer { get; set; } = null!;
    public VehicleVariant VehicleVariant { get; set; } = null!;
    public VehicleColor VehicleColor { get; set; } = null!;
    public User? Approver { get; set; }
}

using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Giao xe
/// </summary>
public class Delivery : BaseEntity
{
    public Guid OrderId { get; set; }
    public string DeliveryCode { get; set; } = string.Empty;
    
    public DateTime ScheduledDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    
    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;
    
    public string DeliveryAddress { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public string ReceiverSignature { get; set; } = string.Empty; // Base64 image
    
    public string DeliveryPhotos { get; set; } = string.Empty; // JSON array of URLs
    public string Notes { get; set; } = string.Empty;
    
    // Navigation properties
    public Order Order { get; set; } = null!;
}

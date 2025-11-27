using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;

public class DeliveryDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string DeliveryCode { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string DealerName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public DeliveryStatus Status { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public string ReceiverSignature { get; set; } = string.Empty;
    public List<string> DeliveryPhotos { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateDeliveryRequest
{
    public Guid OrderId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class UpdateDeliveryStatusRequest
{
    public DeliveryStatus Status { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class UploadDeliveryPhotosRequest
{
    public List<string> PhotoUrls { get; set; } = new();
}

public class CaptureSignatureRequest
{
    public string SignatureBase64 { get; set; } = string.Empty;
    public DateTime SignedAt { get; set; }
}


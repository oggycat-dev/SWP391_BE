using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;

public class VehicleRequestDto
{
    public Guid Id { get; set; }
    public string RequestCode { get; set; } = string.Empty;
    public Guid DealerId { get; set; }
    public string DealerName { get; set; } = string.Empty;
    public Guid VehicleVariantId { get; set; }
    public string VehicleVariantName { get; set; } = string.Empty;
    public string VehicleModelName { get; set; } = string.Empty;
    public Guid VehicleColorId { get; set; }
    public string VehicleColorName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public VehicleRequestStatus Status { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string RequestReason { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class CreateVehicleRequestRequest
{
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    public int Quantity { get; set; }
    public string RequestReason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class ApproveVehicleRequestRequest
{
    public bool Approved { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? RejectionReason { get; set; }
}


namespace CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

/// <summary>
/// Vehicle Variant DTO
/// </summary>
public record VehicleVariantDto
{
    public Guid Id { get; init; }
    public Guid ModelId { get; init; }
    public string VariantName { get; init; } = string.Empty;
    public string VariantCode { get; init; } = string.Empty;
    public Dictionary<string, object> Specifications { get; init; } = new();
    public decimal Price { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    
    // Navigation
    public string ModelName { get; init; } = string.Empty;
}

/// <summary>
/// Create Vehicle Variant Request
/// </summary>
public record CreateVehicleVariantRequest
{
    public Guid ModelId { get; init; }
    public string VariantName { get; init; } = string.Empty;
    public string VariantCode { get; init; } = string.Empty;
    public Dictionary<string, object> Specifications { get; init; } = new();
    public decimal Price { get; init; }
}

/// <summary>
/// Update Vehicle Variant Request
/// </summary>
public record UpdateVehicleVariantRequest
{
    public string VariantName { get; init; } = string.Empty;
    public Dictionary<string, object> Specifications { get; init; } = new();
    public decimal Price { get; init; }
    public bool IsActive { get; init; }
}

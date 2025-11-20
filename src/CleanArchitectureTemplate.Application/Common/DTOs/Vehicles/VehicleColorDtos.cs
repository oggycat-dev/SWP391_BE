namespace CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

/// <summary>
/// Vehicle Color DTO
/// </summary>
public record VehicleColorDto
{
    public Guid Id { get; init; }
    public Guid VariantId { get; init; }
    public string ColorName { get; init; } = string.Empty;
    public string ColorCode { get; init; } = string.Empty;
    public decimal AdditionalPrice { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    
    // Navigation
    public string VariantName { get; init; } = string.Empty;
}

/// <summary>
/// Create Vehicle Color Request
/// </summary>
public record CreateVehicleColorRequest
{
    public Guid VariantId { get; init; }
    public string ColorName { get; init; } = string.Empty;
    public string ColorCode { get; init; } = string.Empty;
    public decimal AdditionalPrice { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
}

/// <summary>
/// Update Vehicle Color Request
/// </summary>
public record UpdateVehicleColorRequest
{
    public string ColorName { get; init; } = string.Empty;
    public string ColorCode { get; init; } = string.Empty;
    public decimal AdditionalPrice { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}

namespace CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

/// <summary>
/// Vehicle Model DTO
/// </summary>
public record VehicleModelDto
{
    public Guid Id { get; init; }
    public string ModelCode { get; init; } = string.Empty;
    public string ModelName { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal BasePrice { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> ImageUrls { get; init; } = new();
    public string BrochureUrl { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ModifiedAt { get; init; }
}

/// <summary>
/// Create Vehicle Model Request
/// </summary>
public record CreateVehicleModelRequest
{
    public string ModelCode { get; init; } = string.Empty;
    public string ModelName { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal BasePrice { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> ImageUrls { get; init; } = new();
    public string BrochureUrl { get; init; } = string.Empty;
}

/// <summary>
/// Update Vehicle Model Request
/// </summary>
public record UpdateVehicleModelRequest
{
    public string ModelName { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal BasePrice { get; init; }
    public string Description { get; init; } = string.Empty;
    public List<string> ImageUrls { get; init; } = new();
    public string BrochureUrl { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}

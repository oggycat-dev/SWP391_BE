namespace CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

/// <summary>
/// Vehicle Inventory DTO
/// </summary>
public record VehicleInventoryDto
{
    public Guid Id { get; init; }
    public Guid VariantId { get; init; }
    public Guid ColorId { get; init; }
    public string VINNumber { get; init; } = string.Empty;
    public string ChassisNumber { get; init; } = string.Empty;
    public string EngineNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string WarehouseLocation { get; init; } = string.Empty;
    public DateTime ManufactureDate { get; init; }
    public DateTime? ImportDate { get; init; }
    public DateTime? SoldDate { get; init; }
    public Guid? DealerId { get; init; }
    public DateTime? AllocatedToDealerDate { get; init; }
    
    // Navigation
    public string ModelName { get; init; } = string.Empty;
    public string VariantName { get; init; } = string.Empty;
    public string ColorName { get; init; } = string.Empty;
    public string? DealerName { get; init; }
}

/// <summary>
/// Create Vehicle Inventory Request
/// </summary>
public record CreateVehicleInventoryRequest
{
    public Guid VariantId { get; init; }
    public Guid ColorId { get; init; }
    public string VINNumber { get; init; } = string.Empty;
    public string ChassisNumber { get; init; } = string.Empty;
    public string EngineNumber { get; init; } = string.Empty;
    public string WarehouseLocation { get; init; } = string.Empty;
    public DateTime ManufactureDate { get; init; }
    public DateTime? ImportDate { get; init; }
}

/// <summary>
/// Update Vehicle Inventory Request
/// </summary>
public record UpdateVehicleInventoryRequest
{
    public string Status { get; init; } = string.Empty;
    public string WarehouseLocation { get; init; } = string.Empty;
    public Guid? DealerId { get; init; }
}

/// <summary>
/// Allocate Vehicle to Dealer Request
/// </summary>
public record AllocateVehicleToDealerRequest
{
    public Guid VehicleInventoryId { get; init; }
    public Guid DealerId { get; init; }
}

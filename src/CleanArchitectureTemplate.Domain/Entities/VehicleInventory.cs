using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Xe cụ thể với VIN (Vehicle Identification Number)
/// </summary>
public class VehicleInventory : BaseEntity
{
    public Guid VariantId { get; set; }
    public Guid ColorId { get; set; }
    
    public string VINNumber { get; set; } = string.Empty; // Unique
    public string ChassisNumber { get; set; } = string.Empty;
    public string EngineNumber { get; set; } = string.Empty;
    
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    public WarehouseLocation WarehouseLocation { get; set; }
    
    public DateTime ManufactureDate { get; set; }
    public DateTime? ImportDate { get; set; }
    public DateTime? SoldDate { get; set; }
    
    // Nếu xe đang ở đại lý
    public Guid? DealerId { get; set; }
    public DateTime? AllocatedToDealerDate { get; set; }
    
    // Navigation properties
    public VehicleVariant Variant { get; set; } = null!;
    public VehicleColor Color { get; set; } = null!;
    public Dealer? Dealer { get; set; }
}

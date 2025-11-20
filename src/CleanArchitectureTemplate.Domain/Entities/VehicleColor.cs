using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Màu sắc xe
/// </summary>
public class VehicleColor : BaseEntity
{
    public Guid VariantId { get; set; }
    public string ColorName { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty; // Hex color
    public decimal AdditionalPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public VehicleVariant Variant { get; set; } = null!;
    public ICollection<VehicleInventory> Inventories { get; set; } = new List<VehicleInventory>();
}

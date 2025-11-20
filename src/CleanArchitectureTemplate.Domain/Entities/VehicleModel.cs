using CleanArchitectureTemplate.Domain.Commons;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Model xe chính (VD: VinFast VF8, Tesla Model 3)
/// </summary>
public class VehicleModel : BaseEntity
{
    public string ModelCode { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public VehicleCategory Category { get; set; }
    public int Year { get; set; }
    public decimal BasePrice { get; set; }
    
    public string Description { get; set; } = string.Empty;
    public string ImageUrls { get; set; } = string.Empty; // JSON array
    public string BrochureUrl { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<VehicleVariant> Variants { get; set; } = new List<VehicleVariant>();
}

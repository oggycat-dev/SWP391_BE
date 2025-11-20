using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Domain.Entities;

/// <summary>
/// Phiên bản xe (VD: Standard, Plus, Eco, Premium)
/// </summary>
public class VehicleVariant : BaseEntity
{
    public Guid ModelId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public string VariantCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Specifications stored as JSON
    /// Example: {
    ///   "batteryCapacity": "87.7 kWh",
    ///   "range": "447 km",
    ///   "chargingTime": "9 hours (AC), 35 mins (DC fast)",
    ///   "motorPower": "300 kW",
    ///   "topSpeed": "200 km/h",
    ///   "acceleration": "5.5s (0-100km/h)",
    ///   "seatingCapacity": 5,
    ///   "dimensions": {
    ///     "length": "4750mm",
    ///     "width": "1934mm",
    ///     "height": "1667mm",
    ///     "wheelbase": "2950mm"
    ///   },
    ///   "features": ["Autopilot", "Panoramic sunroof", "Leather seats", "Wireless charging"]
    /// }
    /// </summary>
    public string Specifications { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public VehicleModel Model { get; set; } = null!;
    public ICollection<VehicleColor> Colors { get; set; } = new List<VehicleColor>();
}

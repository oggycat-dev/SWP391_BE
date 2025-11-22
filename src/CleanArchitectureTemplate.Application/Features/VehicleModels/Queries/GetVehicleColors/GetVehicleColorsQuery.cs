using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColors;

public class GetVehicleColorsQuery : PaginatedQuery<VehicleColorDto>
{
    public Guid? VariantId { get; set; }
    public bool? IsActive { get; set; }
}

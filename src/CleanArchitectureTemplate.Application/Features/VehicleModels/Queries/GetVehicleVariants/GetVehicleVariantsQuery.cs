using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariants;

public class GetVehicleVariantsQuery : PaginatedQuery<VehicleVariantDto>
{
    public Guid? ModelId { get; set; }
    public bool? IsActive { get; set; }
}

using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.CompareVehicles;

public class CompareVehiclesQuery : IRequest<List<VehicleVariantDto>>
{
    public List<Guid> VariantIds { get; set; } = new();
}


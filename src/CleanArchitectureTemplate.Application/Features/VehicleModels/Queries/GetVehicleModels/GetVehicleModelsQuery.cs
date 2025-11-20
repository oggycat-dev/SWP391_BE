using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModels;

public class GetVehicleModelsQuery : PaginatedQuery<VehicleModelDto>
{
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public bool? IsActive { get; set; }
}

using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModelById;

public class GetVehicleModelByIdQuery : IRequest<VehicleModelDto>
{
    public Guid Id { get; set; }
}

using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColorById;

public class GetVehicleColorByIdQuery : IRequest<VehicleColorDto>
{
    public Guid Id { get; set; }
}

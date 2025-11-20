using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventoryById;

public class GetVehicleInventoryByIdQuery : IRequest<VehicleInventoryDto>
{
    public Guid Id { get; set; }
}

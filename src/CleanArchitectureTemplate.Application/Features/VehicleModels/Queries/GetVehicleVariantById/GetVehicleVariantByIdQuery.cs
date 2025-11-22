using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariantById;

public class GetVehicleVariantByIdQuery : IRequest<VehicleVariantDto>
{
    public Guid Id { get; set; }
}

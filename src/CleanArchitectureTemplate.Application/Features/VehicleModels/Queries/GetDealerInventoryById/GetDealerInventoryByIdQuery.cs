using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetDealerInventoryById;

public class GetDealerInventoryByIdQuery : IRequest<VehicleInventoryDto>
{
    public Guid Id { get; set; }
}


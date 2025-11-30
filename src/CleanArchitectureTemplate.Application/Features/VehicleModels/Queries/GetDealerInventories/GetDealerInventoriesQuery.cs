using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetDealerInventories;

public class GetDealerInventoriesQuery : IRequest<List<VehicleInventoryDto>>
{
    public Guid? VariantId { get; set; }
    public Guid? ColorId { get; set; }
    public string? Status { get; set; }
}


using CleanArchitectureTemplate.Application.Common.DTOs.Reports;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Reports.Queries.GetInventoryTurnover;

public class GetInventoryTurnoverQuery : IRequest<List<InventoryTurnoverReport>>
{
    public Guid? VehicleVariantId { get; set; }
}


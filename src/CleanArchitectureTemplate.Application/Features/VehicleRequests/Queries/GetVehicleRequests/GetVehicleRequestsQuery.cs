using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleRequests.Queries.GetVehicleRequests;

public class GetVehicleRequestsQuery : IRequest<List<VehicleRequestDto>>
{
    public Guid? DealerId { get; set; }
    public VehicleRequestStatus? Status { get; set; }
    public bool PendingOnly { get; set; }
}


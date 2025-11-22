using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleRequests.Commands.ApproveVehicleRequest;

public class ApproveVehicleRequestCommand : IRequest<VehicleRequestDto>
{
    public Guid RequestId { get; set; }
    public bool Approved { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? RejectionReason { get; set; }
}


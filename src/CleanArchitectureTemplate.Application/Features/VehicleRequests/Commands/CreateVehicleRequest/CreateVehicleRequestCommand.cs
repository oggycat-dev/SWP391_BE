using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleRequests.Commands.CreateVehicleRequest;

public class CreateVehicleRequestCommand : IRequest<VehicleRequestDto>
{
    public Guid VehicleVariantId { get; set; }
    public Guid VehicleColorId { get; set; }
    public int Quantity { get; set; }
    public string RequestReason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}


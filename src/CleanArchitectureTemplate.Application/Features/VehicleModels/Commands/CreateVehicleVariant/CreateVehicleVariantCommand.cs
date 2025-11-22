using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleVariant;

public class CreateVehicleVariantCommand : IRequest<VehicleVariantDto>
{
    public Guid ModelId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public string VariantCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Dictionary<string, object> Specifications { get; set; } = new();
}

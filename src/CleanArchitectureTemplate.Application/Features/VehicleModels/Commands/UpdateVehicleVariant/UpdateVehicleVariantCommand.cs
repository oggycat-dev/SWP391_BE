using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleVariant;

public class UpdateVehicleVariantCommand : IRequest<VehicleVariantDto>
{
    public Guid Id { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Dictionary<string, object> Specifications { get; set; } = new();
    public bool IsActive { get; set; }
}

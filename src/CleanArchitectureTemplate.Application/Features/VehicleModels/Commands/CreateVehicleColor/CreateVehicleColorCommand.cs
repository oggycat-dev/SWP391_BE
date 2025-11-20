using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleColor;

public class CreateVehicleColorCommand : IRequest<VehicleColorDto>
{
    public Guid VariantId { get; set; }
    public string ColorName { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public string? ImageUrl { get; set; }
}

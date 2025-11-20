using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleColor;

public class UpdateVehicleColorCommand : IRequest<VehicleColorDto>
{
    public Guid Id { get; set; }
    public string ColorName { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
}

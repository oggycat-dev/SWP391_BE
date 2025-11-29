using MediatR;
using Microsoft.AspNetCore.Http;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleModel;

/// <summary>
/// Create vehicle model command
/// </summary>
public class CreateVehicleModelCommand : IRequest<VehicleModelDto>
{
    public string ModelCode { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal BasePrice { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<IFormFile> Images { get; set; } = new();
    public string? BrochureUrl { get; set; }
}

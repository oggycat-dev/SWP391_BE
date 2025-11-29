using MediatR;
using Microsoft.AspNetCore.Http;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleModel;

public class UpdateVehicleModelCommand : IRequest<VehicleModelDto>
{
    public Guid Id { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal BasePrice { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<IFormFile>? NewImages { get; set; }
    public List<string>? ExistingImageUrls { get; set; }
    public string? BrochureUrl { get; set; }
    public bool IsActive { get; set; }
}

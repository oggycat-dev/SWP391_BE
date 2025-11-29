using MediatR;
using AutoMapper;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleModel;

public class UpdateVehicleModelCommandHandler : IRequestHandler<UpdateVehicleModelCommand, VehicleModelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IServiceProvider _serviceProvider;

    public UpdateVehicleModelCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IFileService fileService,
        IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _serviceProvider = serviceProvider;
    }

    public async Task<VehicleModelDto> Handle(UpdateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var vehicleModel = await _unitOfWork.VehicleModels.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle model with ID {request.Id} not found");

        if (!Enum.TryParse<VehicleCategory>(request.Category, out var category))
        {
            throw new ValidationException("Invalid vehicle category");
        }

        // Get old files for deletion
        var oldImageUrls = string.IsNullOrEmpty(vehicleModel.ImageUrls) 
            ? new List<string>() 
            : JsonSerializer.Deserialize<List<string>>(vehicleModel.ImageUrls) ?? new List<string>();

        // Upload new images
        var newImageUrls = new List<string>();
        if (request.NewImages != null && request.NewImages.Any())
        {
            foreach (var image in request.NewImages)
            {
                var uploadResult = await _fileService.UploadFileAsync(image, "vehicles/models");
                newImageUrls.Add(uploadResult.FilePath);
            }
        }

        // Combine existing and new images
        var finalImageUrls = new List<string>();
        if (request.ExistingImageUrls != null && request.ExistingImageUrls.Any())
        {
            finalImageUrls.AddRange(request.ExistingImageUrls);
        }
        finalImageUrls.AddRange(newImageUrls);

        // Update vehicle model
        vehicleModel.ModelName = request.ModelName;
        vehicleModel.Brand = request.Brand;
        vehicleModel.Category = category;
        vehicleModel.Year = request.Year;
        vehicleModel.BasePrice = request.BasePrice;
        vehicleModel.Description = request.Description;
        vehicleModel.ImageUrls = JsonSerializer.Serialize(finalImageUrls);
        vehicleModel.BrochureUrl = request.BrochureUrl ?? string.Empty;
        vehicleModel.IsActive = request.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Delete old images in background (don't block main flow)
        var imagesToDelete = oldImageUrls.Except(request.ExistingImageUrls ?? new List<string>()).ToList();
        if (imagesToDelete.Any())
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
                
                // Delete old images
                foreach (var imageUrl in imagesToDelete)
                {
                    try
                    {
                        await fileService.DeleteFileAsync(imageUrl);
                    }
                    catch
                    {
                        // Log error but don't fail the update
                    }
                }
            });
        }

        var dto = _mapper.Map<VehicleModelDto>(vehicleModel);
        
        // Convert relative paths to full URLs
        dto = dto with
        {
            ImageUrls = dto.ImageUrls.Select(url => _fileService.GetFileUrl(url)).ToList()
        };
        
        return dto;
    }
}

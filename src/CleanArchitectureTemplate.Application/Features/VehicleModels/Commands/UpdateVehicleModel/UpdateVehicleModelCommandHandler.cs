using MediatR;
using AutoMapper;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleModel;

public class UpdateVehicleModelCommandHandler : IRequestHandler<UpdateVehicleModelCommand, VehicleModelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IServiceProvider _serviceProvider;
    private readonly StorageSettings _storageSettings;

    public UpdateVehicleModelCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IFileService fileService,
        IServiceProvider serviceProvider,
        IOptions<StorageSettings> storageSettings)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _serviceProvider = serviceProvider;
        _storageSettings = storageSettings.Value;
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
        
        // If ExistingImageUrls is provided, extract relative paths from full URLs
        if (request.ExistingImageUrls != null && request.ExistingImageUrls.Any())
        {
            // Extract relative paths from full URLs to avoid duplication
            var existingRelativePaths = request.ExistingImageUrls.Select(url => ExtractRelativePath(url)).ToList();
            finalImageUrls.AddRange(existingRelativePaths);
        }
        else if (!newImageUrls.Any())
        {
            // No new images and no ExistingImageUrls provided -> keep old images
            finalImageUrls.AddRange(oldImageUrls);
        }
        else
        {
            // Has new images but no ExistingImageUrls -> keep old images + add new ones
            finalImageUrls.AddRange(oldImageUrls);
        }
        
        // Always add new images if any
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
        // Extract relative paths from existing URLs for comparison
        var existingRelativePathsForComparison = request.ExistingImageUrls?.Select(url => ExtractRelativePath(url)).ToList() ?? new List<string>();
        var imagesToDelete = oldImageUrls.Except(existingRelativePathsForComparison).ToList();
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

    /// <summary>
    /// Extract relative path from full URL or return as-is if already relative
    /// </summary>
    private string ExtractRelativePath(string fileUrlOrPath)
    {
        if (string.IsNullOrEmpty(fileUrlOrPath))
            return string.Empty;

        var baseUrl = _storageSettings.BaseUrl.TrimEnd('/');
        string relativePath;

        // Check if it's a full URL
        if (fileUrlOrPath.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase))
        {
            // Extract relative path from full URL
            relativePath = fileUrlOrPath.Substring(baseUrl.Length).TrimStart('/');
        }
        else
        {
            // It's already a relative path
            relativePath = fileUrlOrPath.TrimStart('/');
        }

        // Remove "uploads/" prefix if present, since we store relative paths without it
        var rootPath = _storageSettings.LocalStorage.RootPath
            .Replace("wwwroot/", "")
            .Replace("wwwroot", "")
            .TrimStart('/');

        if (!string.IsNullOrEmpty(rootPath) && relativePath.StartsWith(rootPath + "/", StringComparison.OrdinalIgnoreCase))
        {
            relativePath = relativePath.Substring(rootPath.Length).TrimStart('/');
        }

        return relativePath;
    }
}

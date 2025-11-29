using MediatR;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleModel;

public class DeleteVehicleModelCommandHandler : IRequestHandler<DeleteVehicleModelCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;

    public DeleteVehicleModelCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _serviceProvider = serviceProvider;
    }

    public async Task<Unit> Handle(DeleteVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var vehicleModel = await _unitOfWork.VehicleModels.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle model with ID {request.Id} not found");

        // Extract files to delete
        var imageUrls = string.IsNullOrEmpty(vehicleModel.ImageUrls) 
            ? new List<string>() 
            : JsonSerializer.Deserialize<List<string>>(vehicleModel.ImageUrls) ?? new List<string>();
        var brochureUrl = vehicleModel.BrochureUrl;

        // Delete the vehicle model from database
        await _unitOfWork.VehicleModels.DeleteAsync(vehicleModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Delete files in background (don't block main flow)
        if (imageUrls.Any() || !string.IsNullOrEmpty(brochureUrl))
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
                
                // Delete images
                foreach (var imageUrl in imageUrls)
                {
                    try
                    {
                        await fileService.DeleteFileAsync(imageUrl);
                    }
                    catch
                    {
                        // Log error but don't fail the delete
                    }
                }

                // Delete brochure
                if (!string.IsNullOrEmpty(brochureUrl))
                {
                    try
                    {
                        await fileService.DeleteFileAsync(brochureUrl);
                    }
                    catch
                    {
                        // Log error but don't fail the delete
                    }
                }
            });
        }

        return Unit.Value;
    }
}

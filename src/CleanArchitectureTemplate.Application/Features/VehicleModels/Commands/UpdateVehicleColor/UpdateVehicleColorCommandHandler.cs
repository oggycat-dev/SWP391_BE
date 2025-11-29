using MediatR;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleColor;

public class UpdateVehicleColorCommandHandler : IRequestHandler<UpdateVehicleColorCommand, VehicleColorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IServiceProvider _serviceProvider;

    public UpdateVehicleColorCommandHandler(
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

    public async Task<VehicleColorDto> Handle(UpdateVehicleColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle color with ID {request.Id} not found");

        var oldImageUrl = color.ImageUrl;

        // Upload new image if provided
        if (request.Image != null)
        {
            var uploadResult = await _fileService.UploadFileAsync(request.Image, "vehicles/colors");
            color.ImageUrl = uploadResult.FilePath; // Store relative path
        }

        color.ColorName = request.ColorName;
        color.ColorCode = request.ColorCode;
        color.AdditionalPrice = request.AdditionalPrice;
        color.IsActive = request.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Delete old image in background if new image was uploaded
        if (request.Image != null && !string.IsNullOrEmpty(oldImageUrl))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
                    await fileService.DeleteFileAsync(oldImageUrl);
                }
                catch { /* Ignore deletion errors */ }
            });
        }

        color = await _unitOfWork.VehicleColors.GetByIdWithVariantAsync(color.Id);
        var dto = _mapper.Map<VehicleColorDto>(color!);
        
        // Convert relative path to full URL
        if (!string.IsNullOrEmpty(dto.ImageUrl))
        {
            dto = dto with { ImageUrl = _fileService.GetFileUrl(dto.ImageUrl) };
        }
        
        return dto;
    }
}

using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleColor;

public class CreateVehicleColorCommandHandler : IRequestHandler<CreateVehicleColorCommand, VehicleColorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public CreateVehicleColorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<VehicleColorDto> Handle(CreateVehicleColorCommand request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VariantId)
            ?? throw new NotFoundException($"Vehicle variant with ID {request.VariantId} not found");

        var existingColor = await _unitOfWork.VehicleColors.GetByColorCodeAsync(request.ColorCode);
        if (existingColor != null)
        {
            throw new ValidationException("Vehicle color with this code already exists");
        }

        // Upload image if provided
        string imageUrl = string.Empty;
        if (request.Image != null)
        {
            var uploadResult = await _fileService.UploadFileAsync(request.Image, "vehicles/colors");
            imageUrl = uploadResult.FilePath; // Store relative path
        }

        var color = new VehicleColor
        {
            Id = Guid.NewGuid(),
            VariantId = request.VariantId,
            ColorName = request.ColorName,
            ColorCode = request.ColorCode,
            AdditionalPrice = request.AdditionalPrice,
            ImageUrl = imageUrl,
            IsActive = true
        };

        await _unitOfWork.VehicleColors.AddAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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

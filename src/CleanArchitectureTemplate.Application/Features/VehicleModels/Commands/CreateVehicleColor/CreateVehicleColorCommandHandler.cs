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

    public CreateVehicleColorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

        var color = new VehicleColor
        {
            Id = Guid.NewGuid(),
            VariantId = request.VariantId,
            ColorName = request.ColorName,
            ColorCode = request.ColorCode,
            AdditionalPrice = request.AdditionalPrice,
            ImageUrl = request.ImageUrl ?? string.Empty,
            IsActive = true
        };

        await _unitOfWork.VehicleColors.AddAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        color = await _unitOfWork.VehicleColors.GetByIdWithVariantAsync(color.Id);
        return _mapper.Map<VehicleColorDto>(color!);
    }
}

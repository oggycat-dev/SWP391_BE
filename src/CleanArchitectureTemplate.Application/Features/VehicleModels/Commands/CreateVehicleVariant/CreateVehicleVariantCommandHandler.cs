using MediatR;
using AutoMapper;
using System.Text.Json;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleVariant;

public class CreateVehicleVariantCommandHandler : IRequestHandler<CreateVehicleVariantCommand, VehicleVariantDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVehicleVariantCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleVariantDto> Handle(CreateVehicleVariantCommand request, CancellationToken cancellationToken)
    {
        // Check if model exists
        var model = await _unitOfWork.VehicleModels.GetByIdAsync(request.ModelId)
            ?? throw new NotFoundException($"Vehicle model with ID {request.ModelId} not found");

        // Check if variant code already exists
        var existingVariant = await _unitOfWork.VehicleVariants.GetByVariantCodeAsync(request.VariantCode);
        if (existingVariant != null)
        {
            throw new ValidationException("Vehicle variant with this code already exists");
        }

        var variant = new VehicleVariant
        {
            Id = Guid.NewGuid(),
            ModelId = request.ModelId,
            VariantName = request.VariantName,
            VariantCode = request.VariantCode,
            Price = request.Price,
            Specifications = JsonSerializer.Serialize(request.Specifications),
            IsActive = true
        };

        await _unitOfWork.VehicleVariants.AddAsync(variant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with model data
        variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(variant.Id);
        return _mapper.Map<VehicleVariantDto>(variant!);
    }
}

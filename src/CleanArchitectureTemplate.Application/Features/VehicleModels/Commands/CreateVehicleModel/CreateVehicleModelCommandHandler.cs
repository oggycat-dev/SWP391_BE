using MediatR;
using AutoMapper;
using System.Text.Json;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleModel;

public class CreateVehicleModelCommandHandler : IRequestHandler<CreateVehicleModelCommand, VehicleModelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVehicleModelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleModelDto> Handle(CreateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        // Check if model code already exists
        var existingModel = await _unitOfWork.VehicleModels.GetByModelCodeAsync(request.ModelCode);
        if (existingModel != null)
        {
            throw new ValidationException("Vehicle model with this code already exists");
        }

        // Parse category
        if (!Enum.TryParse<VehicleCategory>(request.Category, out var category))
        {
            throw new ValidationException("Invalid vehicle category");
        }

        // Create vehicle model
        var vehicleModel = new VehicleModel
        {
            Id = Guid.NewGuid(),
            ModelCode = request.ModelCode,
            ModelName = request.ModelName,
            Brand = request.Brand,
            Category = category,
            Year = request.Year,
            BasePrice = request.BasePrice,
            Description = request.Description,
            ImageUrls = JsonSerializer.Serialize(request.ImageUrls),
            BrochureUrl = request.BrochureUrl,
            IsActive = true
        };

        await _unitOfWork.VehicleModels.AddAsync(vehicleModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleModelDto>(vehicleModel);
    }
}

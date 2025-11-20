using MediatR;
using AutoMapper;
using System.Text.Json;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleModel;

public class UpdateVehicleModelCommandHandler : IRequestHandler<UpdateVehicleModelCommand, VehicleModelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVehicleModelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleModelDto> Handle(UpdateVehicleModelCommand request, CancellationToken cancellationToken)
    {
        var vehicleModel = await _unitOfWork.VehicleModels.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle model with ID {request.Id} not found");

        if (!Enum.TryParse<VehicleCategory>(request.Category, out var category))
        {
            throw new ValidationException("Invalid vehicle category");
        }

        vehicleModel.ModelName = request.ModelName;
        vehicleModel.Brand = request.Brand;
        vehicleModel.Category = category;
        vehicleModel.Year = request.Year;
        vehicleModel.BasePrice = request.BasePrice;
        vehicleModel.Description = request.Description;
        vehicleModel.ImageUrls = JsonSerializer.Serialize(request.ImageUrls);
        vehicleModel.BrochureUrl = request.BrochureUrl;
        vehicleModel.IsActive = request.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleModelDto>(vehicleModel);
    }
}

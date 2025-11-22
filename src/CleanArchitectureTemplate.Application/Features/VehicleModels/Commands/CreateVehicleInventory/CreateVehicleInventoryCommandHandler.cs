using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.CreateVehicleInventory;

public class CreateVehicleInventoryCommandHandler : IRequestHandler<CreateVehicleInventoryCommand, VehicleInventoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateVehicleInventoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleInventoryDto> Handle(CreateVehicleInventoryCommand request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VariantId)
            ?? throw new NotFoundException($"Vehicle variant with ID {request.VariantId} not found");

        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.ColorId)
            ?? throw new NotFoundException($"Vehicle color with ID {request.ColorId} not found");

        if (color.VariantId != request.VariantId)
        {
            throw new ValidationException("Color does not belong to the specified variant");
        }

        // Check VIN uniqueness
        var existingVIN = await _unitOfWork.VehicleInventories.GetByVINAsync(request.VIN);
        if (existingVIN != null)
        {
            throw new ValidationException("Vehicle with this VIN already exists");
        }

        if (!Enum.TryParse<WarehouseLocation>(request.WarehouseLocation, out var location))
        {
            throw new ValidationException("Invalid warehouse location");
        }

        var inventory = new VehicleInventory
        {
            Id = Guid.NewGuid(),
            VariantId = request.VariantId,
            ColorId = request.ColorId,
            VINNumber = request.VIN,
            ChassisNumber = request.ChassisNumber,
            EngineNumber = request.EngineNumber,
            ManufactureDate = request.ManufactureDate,
            ImportDate = request.ImportDate,
            Status = VehicleStatus.Available,
            WarehouseLocation = location
        };

        await _unitOfWork.VehicleInventories.AddAsync(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        inventory = await _unitOfWork.VehicleInventories.GetByIdWithDetailsAsync(inventory.Id);
        return _mapper.Map<VehicleInventoryDto>(inventory!);
    }
}

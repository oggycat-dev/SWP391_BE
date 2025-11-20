using MediatR;
using AutoMapper;
using System.Text.Json;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleVariant;

public class UpdateVehicleVariantCommandHandler : IRequestHandler<UpdateVehicleVariantCommand, VehicleVariantDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVehicleVariantCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleVariantDto> Handle(UpdateVehicleVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle variant with ID {request.Id} not found");

        variant.VariantName = request.VariantName;
        variant.Price = request.Price;
        variant.Specifications = JsonSerializer.Serialize(request.Specifications);
        variant.IsActive = request.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with model data
        variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(variant.Id);
        return _mapper.Map<VehicleVariantDto>(variant!);
    }
}

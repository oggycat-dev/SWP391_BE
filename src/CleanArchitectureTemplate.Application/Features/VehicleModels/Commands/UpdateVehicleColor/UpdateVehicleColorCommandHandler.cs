using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.UpdateVehicleColor;

public class UpdateVehicleColorCommandHandler : IRequestHandler<UpdateVehicleColorCommand, VehicleColorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVehicleColorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleColorDto> Handle(UpdateVehicleColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle color with ID {request.Id} not found");

        color.ColorName = request.ColorName;
        color.ColorCode = request.ColorCode;
        color.AdditionalPrice = request.AdditionalPrice;
        color.ImageUrl = request.ImageUrl ?? string.Empty;
        color.IsActive = request.IsActive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        color = await _unitOfWork.VehicleColors.GetByIdWithVariantAsync(color.Id);
        return _mapper.Map<VehicleColorDto>(color!);
    }
}

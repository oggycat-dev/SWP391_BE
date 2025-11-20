using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleVariantById;

public class GetVehicleVariantByIdQueryHandler : IRequestHandler<GetVehicleVariantByIdQuery, VehicleVariantDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleVariantByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleVariantDto> Handle(GetVehicleVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle variant with ID {request.Id} not found");

        return _mapper.Map<VehicleVariantDto>(variant);
    }
}

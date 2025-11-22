using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColorById;

public class GetVehicleColorByIdQueryHandler : IRequestHandler<GetVehicleColorByIdQuery, VehicleColorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleColorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleColorDto> Handle(GetVehicleColorByIdQuery request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.VehicleColors.GetByIdWithVariantAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle color with ID {request.Id} not found");

        return _mapper.Map<VehicleColorDto>(color);
    }
}

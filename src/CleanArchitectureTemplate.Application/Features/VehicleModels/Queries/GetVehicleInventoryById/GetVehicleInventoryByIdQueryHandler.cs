using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleInventoryById;

public class GetVehicleInventoryByIdQueryHandler : IRequestHandler<GetVehicleInventoryByIdQuery, VehicleInventoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleInventoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleInventoryDto> Handle(GetVehicleInventoryByIdQuery request, CancellationToken cancellationToken)
    {
        var inventory = await _unitOfWork.VehicleInventories.GetByIdWithDetailsAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle inventory with ID {request.Id} not found");

        return _mapper.Map<VehicleInventoryDto>(inventory);
    }
}

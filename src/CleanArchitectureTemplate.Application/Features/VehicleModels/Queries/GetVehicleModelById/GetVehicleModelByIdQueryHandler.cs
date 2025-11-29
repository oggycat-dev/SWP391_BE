using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModelById;

public class GetVehicleModelByIdQueryHandler : IRequestHandler<GetVehicleModelByIdQuery, VehicleModelDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public GetVehicleModelByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<VehicleModelDto> Handle(GetVehicleModelByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicleModel = await _unitOfWork.VehicleModels.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle model with ID {request.Id} not found");

        var dto = _mapper.Map<VehicleModelDto>(vehicleModel);
        
        // Convert relative paths to full URLs
        dto = dto with
        {
            ImageUrls = dto.ImageUrls.Select(url => _fileService.GetFileUrl(url)).ToList()
        };
        
        return dto;
    }
}

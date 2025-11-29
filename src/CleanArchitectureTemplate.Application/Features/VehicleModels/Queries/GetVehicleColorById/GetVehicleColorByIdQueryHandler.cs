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
    private readonly IFileService _fileService;

    public GetVehicleColorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<VehicleColorDto> Handle(GetVehicleColorByIdQuery request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.VehicleColors.GetByIdWithVariantAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle color with ID {request.Id} not found");

        var dto = _mapper.Map<VehicleColorDto>(color);
        
        // Convert relative path to full URL
        if (!string.IsNullOrEmpty(dto.ImageUrl))
        {
            dto = dto with { ImageUrl = _fileService.GetFileUrl(dto.ImageUrl) };
        }
        
        return dto;
    }
}

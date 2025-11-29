using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleColors;

public class GetVehicleColorsQueryHandler : IRequestHandler<GetVehicleColorsQuery, PaginatedResult<VehicleColorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public GetVehicleColorsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<PaginatedResult<VehicleColorDto>> Handle(GetVehicleColorsQuery request, CancellationToken cancellationToken)
    {
        var colors = await _unitOfWork.VehicleColors.GetAllWithVariantsAsync();

        var filtered = colors.AsEnumerable();

        if (request.VariantId.HasValue)
        {
            filtered = filtered.Where(c => c.VariantId == request.VariantId.Value);
        }

        if (request.IsActive.HasValue)
        {
            filtered = filtered.Where(c => c.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filtered = filtered.Where(c =>
                c.ColorName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.ColorCode.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var total = filtered.Count();
        var items = filtered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<VehicleColorDto>>(items);
        
        // Convert relative paths to full URLs for all items
        dtos = dtos.Select(dto => 
        {
            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                return dto with { ImageUrl = _fileService.GetFileUrl(dto.ImageUrl) };
            }
            return dto;
        }).ToList();

        return new PaginatedResult<VehicleColorDto>
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

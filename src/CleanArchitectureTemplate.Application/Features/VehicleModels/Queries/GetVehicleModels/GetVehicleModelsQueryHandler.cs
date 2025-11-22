using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Models;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetVehicleModels;

public class GetVehicleModelsQueryHandler : IRequestHandler<GetVehicleModelsQuery, PaginatedResult<VehicleModelDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleModelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<VehicleModelDto>> Handle(GetVehicleModelsQuery request, CancellationToken cancellationToken)
    {
        var vehicleModels = await _unitOfWork.VehicleModels.GetAllAsync();

        // Apply filters
        var filtered = vehicleModels.AsQueryable();

        if (!string.IsNullOrEmpty(request.Brand))
        {
            filtered = filtered.Where(vm => vm.Brand.Contains(request.Brand, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            filtered = filtered.Where(vm => vm.Category.ToString() == request.Category);
        }

        if (request.IsActive.HasValue)
        {
            filtered = filtered.Where(vm => vm.IsActive == request.IsActive.Value);
        }

        // Apply search
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            filtered = filtered.Where(vm =>
                vm.ModelName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                vm.ModelCode.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                vm.Brand.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var total = filtered.Count();
        var items = filtered
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<VehicleModelDto>>(items);
        
        return new PaginatedResult<VehicleModelDto>
        {
            Items = dtos,
            TotalCount = total,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

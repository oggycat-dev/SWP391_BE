using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetDealerInventories;

public class GetDealerInventoriesQueryHandler : IRequestHandler<GetDealerInventoriesQuery, List<VehicleInventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetDealerInventoriesQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<VehicleInventoryDto>> Handle(GetDealerInventoriesQuery request, CancellationToken cancellationToken)
    {
        // Get current dealer staff
        var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
        if (dealerStaff == null)
        {
            throw new UnauthorizedAccessException("User is not associated with any dealer.");
        }

        // Get inventories for this dealer
        var inventories = await _unitOfWork.VehicleInventories.GetByDealerIdAsync(dealerStaff.DealerId);

        // Apply filters
        var filtered = inventories.AsEnumerable();

        if (request.VariantId.HasValue)
        {
            filtered = filtered.Where(i => i.VariantId == request.VariantId.Value);
        }

        if (request.ColorId.HasValue)
        {
            filtered = filtered.Where(i => i.ColorId == request.ColorId.Value);
        }

        if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<VehicleStatus>(request.Status, out var status))
        {
            filtered = filtered.Where(i => i.Status == status);
        }

        var result = filtered.ToList();
        return _mapper.Map<List<VehicleInventoryDto>>(result);
    }
}


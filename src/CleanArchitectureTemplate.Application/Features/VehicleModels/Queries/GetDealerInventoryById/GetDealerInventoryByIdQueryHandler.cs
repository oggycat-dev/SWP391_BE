using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.GetDealerInventoryById;

public class GetDealerInventoryByIdQueryHandler : IRequestHandler<GetDealerInventoryByIdQuery, VehicleInventoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetDealerInventoryByIdQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<VehicleInventoryDto> Handle(GetDealerInventoryByIdQuery request, CancellationToken cancellationToken)
    {
        // Get current dealer staff
        var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
        if (dealerStaff == null)
        {
            throw new UnauthorizedAccessException("User is not associated with any dealer.");
        }

        // Get inventory
        var inventory = await _unitOfWork.VehicleInventories.GetByIdWithDetailsAsync(request.Id);
        if (inventory == null)
        {
            throw new NotFoundException($"Vehicle inventory with ID {request.Id} not found");
        }

        // Verify inventory belongs to this dealer
        if (!inventory.DealerId.HasValue || inventory.DealerId.Value != dealerStaff.DealerId)
        {
            throw new UnauthorizedAccessException("You do not have access to this inventory.");
        }

        return _mapper.Map<VehicleInventoryDto>(inventory);
    }
}


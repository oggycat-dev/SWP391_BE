using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleRequests.Commands.CreateVehicleRequest;

public class CreateVehicleRequestCommandHandler : IRequestHandler<CreateVehicleRequestCommand, VehicleRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateVehicleRequestCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<VehicleRequestDto> Handle(CreateVehicleRequestCommand request, CancellationToken cancellationToken)
    {
        // Get current dealer staff
        var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
        if (dealerStaff == null)
        {
            throw new UnauthorizedAccessException("User is not associated with any dealer.");
        }

        // Get dealer information
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(dealerStaff.DealerId);
        if (dealer == null)
        {
            throw new NotFoundException("Dealer", dealerStaff.DealerId);
        }

        // Check dealer debt limit
        var totalDebt = await _unitOfWork.DealerDebts.GetTotalDebtByDealerAsync(dealerStaff.DealerId);
        
        if (totalDebt >= dealer.DebtLimit)
        {
            throw new ValidationException("Dealer has exceeded debt limit. Cannot request new vehicles.");
        }

        // Validate vehicle variant and color
        var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VehicleVariantId);
        if (variant == null)
        {
            throw new NotFoundException("Vehicle Variant", request.VehicleVariantId);
        }

        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.VehicleColorId);
        if (color == null || color.VariantId != request.VehicleVariantId)
        {
            throw new ValidationException("Invalid vehicle color for the selected variant.");
        }

        var requestCode = await GenerateRequestCodeAsync();

        var vehicleRequest = new VehicleRequest
        {
            RequestCode = requestCode,
            DealerId = dealerStaff.DealerId,
            VehicleVariantId = request.VehicleVariantId,
            VehicleColorId = request.VehicleColorId,
            Quantity = request.Quantity,
            Status = VehicleRequestStatus.Pending,
            RequestDate = DateTime.UtcNow,
            RequestReason = request.RequestReason,
            Notes = request.Notes
        };

        await _unitOfWork.VehicleRequests.AddAsync(vehicleRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _unitOfWork.VehicleRequests.GetByIdAsync(vehicleRequest.Id);
        return _mapper.Map<VehicleRequestDto>(result);
    }

    private async Task<string> GenerateRequestCodeAsync()
    {
        var count = (await _unitOfWork.VehicleRequests.GetAllAsync()).Count;
        return $"VRQ{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleRequests.Commands.ApproveVehicleRequest;

public class ApproveVehicleRequestCommandHandler : IRequestHandler<ApproveVehicleRequestCommand, VehicleRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public ApproveVehicleRequestCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<VehicleRequestDto> Handle(ApproveVehicleRequestCommand request, CancellationToken cancellationToken)
    {
        var vehicleRequest = await _unitOfWork.VehicleRequests.GetByIdAsync(request.RequestId);
        if (vehicleRequest == null)
        {
            throw new NotFoundException("Vehicle Request", request.RequestId);
        }

        if (vehicleRequest.Status != VehicleRequestStatus.Pending)
        {
            throw new ValidationException("Only pending requests can be approved or rejected.");
        }

        vehicleRequest.Status = request.Approved ? VehicleRequestStatus.Approved : VehicleRequestStatus.Rejected;
        vehicleRequest.ApprovedDate = DateTime.UtcNow;
        vehicleRequest.ApprovedBy = _currentUserService.UserId;
        vehicleRequest.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
        vehicleRequest.RejectionReason = request.RejectionReason;

        await _unitOfWork.VehicleRequests.UpdateAsync(vehicleRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleRequestDto>(vehicleRequest);
    }
}


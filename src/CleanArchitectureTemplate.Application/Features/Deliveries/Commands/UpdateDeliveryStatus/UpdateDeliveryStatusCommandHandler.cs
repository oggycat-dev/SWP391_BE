using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.UpdateDeliveryStatus;

public class UpdateDeliveryStatusCommandHandler : IRequestHandler<UpdateDeliveryStatusCommand, DeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDeliveryStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeliveryDto> Handle(UpdateDeliveryStatusCommand request, CancellationToken cancellationToken)
    {
        var delivery = await _unitOfWork.Deliveries.GetByIdAsync(request.Id);
        if (delivery == null)
        {
            throw new NotFoundException(nameof(Delivery), request.Id);
        }

        // Validate status transition
        ValidateStatusTransition(delivery.Status, request.Status);

        delivery.Status = request.Status;
        delivery.Notes = request.Notes;

        if (request.Status == DeliveryStatus.Delivered && request.ActualDeliveryDate.HasValue)
        {
            delivery.ActualDeliveryDate = request.ActualDeliveryDate.Value;
        }

        await _unitOfWork.Deliveries.UpdateAsync(delivery);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details
        var deliveryWithDetails = await _unitOfWork.Deliveries.GetByIdWithDetailsAsync(delivery.Id);
        
        return _mapper.Map<DeliveryDto>(deliveryWithDetails);
    }

    private void ValidateStatusTransition(DeliveryStatus currentStatus, DeliveryStatus newStatus)
    {
        // Pending -> InTransit -> Delivered or Failed
        if (currentStatus == DeliveryStatus.Pending && newStatus != DeliveryStatus.InTransit && newStatus != DeliveryStatus.Failed)
        {
            throw new ValidationException($"Cannot change status from {currentStatus} to {newStatus}");
        }

        if (currentStatus == DeliveryStatus.InTransit && newStatus != DeliveryStatus.Delivered && newStatus != DeliveryStatus.Failed)
        {
            throw new ValidationException($"Cannot change status from {currentStatus} to {newStatus}");
        }

        if (currentStatus == DeliveryStatus.Delivered || currentStatus == DeliveryStatus.Failed)
        {
            throw new ValidationException($"Cannot change status from {currentStatus}");
        }
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CreateDelivery;

public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, DeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDeliveryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeliveryDto> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        // Verify order exists
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        // Check if delivery already exists for this order
        var existingDelivery = await _unitOfWork.Deliveries.GetByOrderIdAsync(request.OrderId);
        if (existingDelivery != null)
        {
            throw new ValidationException("Delivery already exists for this order");
        }

        // Generate delivery code
        var deliveryCode = await GenerateDeliveryCodeAsync();

        var delivery = new Delivery
        {
            DeliveryCode = deliveryCode,
            OrderId = request.OrderId,
            ScheduledDate = request.ScheduledDate,
            DeliveryAddress = request.DeliveryAddress,
            ReceiverName = request.ReceiverName,
            ReceiverPhone = request.ReceiverPhone,
            Notes = request.Notes,
            Status = DeliveryStatus.Pending
        };

        await _unitOfWork.Deliveries.AddAsync(delivery);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details for mapping
        var deliveryWithDetails = await _unitOfWork.Deliveries.GetByIdWithDetailsAsync(delivery.Id);
        
        return _mapper.Map<DeliveryDto>(deliveryWithDetails);
    }

    private async Task<string> GenerateDeliveryCodeAsync()
    {
        var count = (await _unitOfWork.Deliveries.GetAllAsync()).Count;
        return $"DEL{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException("Order", request.OrderId);
        }

        // Business rule validation based on status transitions
        ValidateStatusTransition(order.Status, request.Status);

        order.Status = request.Status;
        
        if (request.Status == OrderStatus.Approved)
        {
            order.ApprovedDate = DateTime.UtcNow;
        }
        else if (request.Status == OrderStatus.Delivered)
        {
            order.DeliveryDate = DateTime.UtcNow;
            
            // Update vehicle inventory status if allocated
            if (order.VehicleInventoryId.HasValue)
            {
                var vehicle = await _unitOfWork.VehicleInventories.GetByIdAsync(order.VehicleInventoryId.Value);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Sold;
                    vehicle.SoldDate = DateTime.UtcNow;
                    await _unitOfWork.VehicleInventories.UpdateAsync(vehicle);
                }
            }
        }

        if (!string.IsNullOrEmpty(request.Notes))
        {
            order.Notes += $"\n[{DateTime.UtcNow:yyyy-MM-dd HH:mm}] Status updated to {request.Status}: {request.Notes}";
        }

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _unitOfWork.Orders.GetByIdWithDetailsAsync(order.Id);
        return _mapper.Map<OrderDto>(result);
    }

    private void ValidateStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        // Define valid status transitions
        var validTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
        {
            { OrderStatus.Draft, new List<OrderStatus> { OrderStatus.Pending, OrderStatus.Cancelled } },
            { OrderStatus.Pending, new List<OrderStatus> { OrderStatus.Approved, OrderStatus.Cancelled } },
            { OrderStatus.Approved, new List<OrderStatus> { OrderStatus.VehicleAllocated, OrderStatus.Cancelled } },
            { OrderStatus.VehicleAllocated, new List<OrderStatus> { OrderStatus.InDelivery, OrderStatus.Cancelled } },
            { OrderStatus.InDelivery, new List<OrderStatus> { OrderStatus.Delivered } },
            { OrderStatus.Delivered, new List<OrderStatus> { OrderStatus.Completed } },
            { OrderStatus.Completed, new List<OrderStatus>() },
            { OrderStatus.Cancelled, new List<OrderStatus>() }
        };

        if (!validTransitions[currentStatus].Contains(newStatus))
        {
            throw new ValidationException($"Invalid status transition from {currentStatus} to {newStatus}.");
        }
    }
}


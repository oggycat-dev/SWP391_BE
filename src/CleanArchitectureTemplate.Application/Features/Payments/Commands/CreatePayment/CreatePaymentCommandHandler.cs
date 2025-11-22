using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePaymentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // Validate order
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException("Order", request.OrderId);
        }

        // Validate payment amount
        if (request.Amount <= 0)
        {
            throw new ValidationException("Payment amount must be greater than zero.");
        }

        if (request.Amount > order.RemainingAmount)
        {
            throw new ValidationException($"Payment amount cannot exceed remaining amount of {order.RemainingAmount}.");
        }

        // Generate payment code
        var paymentCode = await GeneratePaymentCodeAsync();

        var payment = new Payment
        {
            PaymentCode = paymentCode,
            OrderId = request.OrderId,
            Amount = request.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = request.Method,
            Status = PaymentStatus.Completed,
            TransactionReference = request.TransactionReference,
            Notes = request.Notes
        };

        await _unitOfWork.Payments.AddAsync(payment);

        // Update order payment status
        order.PaidAmount += request.Amount;
        order.RemainingAmount -= request.Amount;

        if (order.RemainingAmount <= 0)
        {
            order.PaymentStatus = PaymentStatus.Completed;
        }
        else
        {
            order.PaymentStatus = PaymentStatus.Partial;
        }

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PaymentDto>(payment);
    }

    private async Task<string> GeneratePaymentCodeAsync()
    {
        var count = (await _unitOfWork.Payments.GetAllAsync()).Count;
        return $"PAY{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Payments.Queries.GetPaymentsByOrder;

public class GetPaymentsByOrderQueryHandler : IRequestHandler<GetPaymentsByOrderQuery, PaymentSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaymentsByOrderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaymentSummaryDto> Handle(GetPaymentsByOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException("Order", request.OrderId);
        }

        var payments = await _unitOfWork.Payments.GetByOrderIdAsync(request.OrderId);
        var installmentPlan = order.IsInstallment 
            ? await _unitOfWork.InstallmentPlans.GetByOrderIdAsync(request.OrderId)
            : null;

        return new PaymentSummaryDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            TotalAmount = order.TotalAmount,
            PaidAmount = order.PaidAmount,
            RemainingAmount = order.RemainingAmount,
            PaymentStatus = order.PaymentStatus.ToString(),
            IsInstallment = order.IsInstallment,
            Payments = _mapper.Map<List<PaymentDto>>(payments),
            InstallmentPlan = installmentPlan != null ? _mapper.Map<InstallmentPlanDto>(installmentPlan) : null
        };
    }
}


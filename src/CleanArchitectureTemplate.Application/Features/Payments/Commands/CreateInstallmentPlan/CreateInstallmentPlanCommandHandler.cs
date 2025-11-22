using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Payments.Commands.CreateInstallmentPlan;

public class CreateInstallmentPlanCommandHandler : IRequestHandler<CreateInstallmentPlanCommand, InstallmentPlanDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateInstallmentPlanCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<InstallmentPlanDto> Handle(CreateInstallmentPlanCommand request, CancellationToken cancellationToken)
    {
        // Validate order
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException("Order", request.OrderId);
        }

        if (!order.IsInstallment)
        {
            throw new ValidationException("Order is not marked for installment payment.");
        }

        // Business rules validation
        var minDownPayment = order.TotalAmount * 0.20m; // 20% minimum
        if (request.DownPayment < minDownPayment)
        {
            throw new ValidationException($"Down payment must be at least 20% ({minDownPayment:C}).");
        }

        var validTerms = new[] { 12, 24, 36, 48 };
        if (!validTerms.Contains(request.TermMonths))
        {
            throw new ValidationException("Valid term months are: 12, 24, 36, or 48.");
        }

        // Calculate installment plan
        var loanAmount = order.TotalAmount - request.DownPayment;
        var monthlyInterestRate = request.InterestRate / 100 / 12;
        var monthlyPayment = (loanAmount * monthlyInterestRate) / 
                            (1 - (decimal)Math.Pow(1 + (double)monthlyInterestRate, -request.TermMonths));

        var planCode = await GeneratePlanCodeAsync();

        var installmentPlan = new InstallmentPlan
        {
            PlanCode = planCode,
            OrderId = request.OrderId,
            TotalAmount = order.TotalAmount,
            DownPayment = request.DownPayment,
            LoanAmount = loanAmount,
            TermMonths = request.TermMonths,
            InterestRate = request.InterestRate,
            MonthlyPayment = monthlyPayment,
            PaidAmount = request.DownPayment,
            RemainingAmount = loanAmount,
            NextPaymentDate = DateTime.UtcNow.AddMonths(1),
            BankName = request.BankName,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(request.TermMonths)
        };

        await _unitOfWork.InstallmentPlans.AddAsync(installmentPlan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InstallmentPlanDto>(installmentPlan);
    }

    private async Task<string> GeneratePlanCodeAsync()
    {
        var count = (await _unitOfWork.InstallmentPlans.GetAllAsync()).Count;
        return $"INS{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


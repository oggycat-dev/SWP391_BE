using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Commands.PayDealerDebt;

public class PayDealerDebtCommandHandler : IRequestHandler<PayDealerDebtCommand, DealerDebtDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PayDealerDebtCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDebtDto> Handle(PayDealerDebtCommand request, CancellationToken cancellationToken)
    {
        var debt = await _unitOfWork.DealerDebts.GetByIdAsync(request.Id);
        if (debt == null)
        {
            throw new KeyNotFoundException($"Dealer debt with ID {request.Id} not found");
        }

        if (request.PaymentAmount <= 0)
        {
            throw new ArgumentException("Payment amount must be greater than zero");
        }

        if (request.PaymentAmount > debt.RemainingAmount)
        {
            throw new ArgumentException("Payment amount exceeds remaining debt");
        }

        debt.PaidAmount += request.PaymentAmount;
        debt.RemainingAmount -= request.PaymentAmount;
        debt.Notes = string.IsNullOrEmpty(debt.Notes) ? request.Notes : $"{debt.Notes}\n{request.Notes}";
        debt.ModifiedAt = DateTime.UtcNow;

        // Update status
        if (debt.RemainingAmount == 0)
        {
            debt.Status = DebtStatus.Paid;
        }
        else if (debt.DueDate < DateTime.UtcNow)
        {
            debt.Status = DebtStatus.Overdue;
        }
        else
        {
            debt.Status = DebtStatus.Current;
        }

        await _unitOfWork.DealerDebts.UpdateAsync(debt);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dealer = await _unitOfWork.Dealers.GetByIdAsync(debt.DealerId);
        var result = _mapper.Map<DealerDebtDto>(debt);
        result.DealerName = dealer?.Name ?? string.Empty;
        
        return result;
    }
}


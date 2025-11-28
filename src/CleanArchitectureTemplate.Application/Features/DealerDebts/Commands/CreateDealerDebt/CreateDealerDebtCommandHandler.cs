using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Commands.CreateDealerDebt;

public class CreateDealerDebtCommandHandler : IRequestHandler<CreateDealerDebtCommand, DealerDebtDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDealerDebtCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDebtDto> Handle(CreateDealerDebtCommand request, CancellationToken cancellationToken)
    {
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId);
        if (dealer == null)
        {
            throw new KeyNotFoundException($"Dealer with ID {request.DealerId} not found");
        }

        // Generate debt code
        var debtCount = (await _unitOfWork.DealerDebts.GetByDealerIdAsync(request.DealerId)).Count;
        var debtCode = $"DD-{dealer.DealerCode}-{DateTime.UtcNow:yyyyMMdd}-{(debtCount + 1):D3}";

        var debt = new DealerDebt
        {
            Id = Guid.NewGuid(),
            DealerId = request.DealerId,
            DebtCode = debtCode,
            TotalDebt = request.TotalDebt,
            PaidAmount = 0,
            RemainingAmount = request.TotalDebt,
            DueDate = request.DueDate,
            Status = DebtStatus.Current,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DealerDebts.AddAsync(debt);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<DealerDebtDto>(debt);
        result.DealerName = dealer.Name;
        
        return result;
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.CreateDealerContract;

public class CreateDealerContractCommandHandler : IRequestHandler<CreateDealerContractCommand, DealerContractDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDealerContractCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerContractDto> Handle(CreateDealerContractCommand request, CancellationToken cancellationToken)
    {
        // Verify dealer exists
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId);
        if (dealer == null)
        {
            throw new KeyNotFoundException($"Dealer with ID {request.DealerId} not found");
        }

        // Generate contract number
        var contractCount = (await _unitOfWork.DealerContracts.GetByDealerIdAsync(request.DealerId)).Count;
        var contractNumber = $"DC-{dealer.DealerCode}-{DateTime.UtcNow:yyyyMMdd}-{(contractCount + 1):D3}";

        var contract = new DealerContract
        {
            Id = Guid.NewGuid(),
            DealerId = request.DealerId,
            ContractNumber = contractNumber,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Terms = request.Terms,
            CommissionRate = request.CommissionRate,
            Status = DealerContractStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DealerContracts.AddAsync(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<DealerContractDto>(contract);
        result.DealerName = dealer.Name;
        
        return result;
    }
}


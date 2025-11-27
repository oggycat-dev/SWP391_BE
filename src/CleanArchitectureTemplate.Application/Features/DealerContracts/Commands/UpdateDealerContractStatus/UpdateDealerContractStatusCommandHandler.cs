using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Commands.UpdateDealerContractStatus;

public class UpdateDealerContractStatusCommandHandler : IRequestHandler<UpdateDealerContractStatusCommand, DealerContractDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDealerContractStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerContractDto> Handle(UpdateDealerContractStatusCommand request, CancellationToken cancellationToken)
    {
        var contract = await _unitOfWork.DealerContracts.GetByIdAsync(request.Id);
        if (contract == null)
        {
            throw new KeyNotFoundException($"Dealer contract with ID {request.Id} not found");
        }

        contract.Status = request.Status;
        
        // If status is Active, record signing information
        if (request.Status == DealerContractStatus.Active && 
            !string.IsNullOrEmpty(request.SignedBy))
        {
            contract.SignedBy = request.SignedBy;
            contract.SignedDate = DateTime.UtcNow;
        }

        contract.ModifiedAt = DateTime.UtcNow;
        
        await _unitOfWork.DealerContracts.UpdateAsync(contract);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dealer = await _unitOfWork.Dealers.GetByIdAsync(contract.DealerId);
        var result = _mapper.Map<DealerContractDto>(contract);
        result.DealerName = dealer?.Name ?? string.Empty;
        
        return result;
    }
}


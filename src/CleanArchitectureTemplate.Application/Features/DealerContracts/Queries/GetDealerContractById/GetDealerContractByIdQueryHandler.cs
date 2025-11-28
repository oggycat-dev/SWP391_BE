using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Queries.GetDealerContractById;

public class GetDealerContractByIdQueryHandler : IRequestHandler<GetDealerContractByIdQuery, DealerContractDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerContractByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerContractDto> Handle(GetDealerContractByIdQuery request, CancellationToken cancellationToken)
    {
        var contract = await _unitOfWork.DealerContracts.GetByIdAsync(request.Id);
        if (contract == null)
        {
            throw new KeyNotFoundException($"Dealer contract with ID {request.Id} not found");
        }

        var dealer = await _unitOfWork.Dealers.GetByIdAsync(contract.DealerId);
        var result = _mapper.Map<DealerContractDto>(contract);
        result.DealerName = dealer?.Name ?? string.Empty;
        
        return result;
    }
}


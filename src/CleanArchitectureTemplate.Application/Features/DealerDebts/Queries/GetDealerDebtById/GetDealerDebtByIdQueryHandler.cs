using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Queries.GetDealerDebtById;

public class GetDealerDebtByIdQueryHandler : IRequestHandler<GetDealerDebtByIdQuery, DealerDebtDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerDebtByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDebtDto> Handle(GetDealerDebtByIdQuery request, CancellationToken cancellationToken)
    {
        var debt = await _unitOfWork.DealerDebts.GetByIdAsync(request.Id);
        if (debt == null)
        {
            throw new KeyNotFoundException($"Dealer debt with ID {request.Id} not found");
        }

        var dealer = await _unitOfWork.Dealers.GetByIdAsync(debt.DealerId);
        var result = _mapper.Map<DealerDebtDto>(debt);
        result.DealerName = dealer?.Name ?? string.Empty;
        
        return result;
    }
}


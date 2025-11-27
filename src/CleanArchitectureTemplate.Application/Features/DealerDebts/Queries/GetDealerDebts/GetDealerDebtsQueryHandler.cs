using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerDebts.Queries.GetDealerDebts;

public class GetDealerDebtsQueryHandler : IRequestHandler<GetDealerDebtsQuery, List<DealerDebtDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerDebtsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<DealerDebtDto>> Handle(GetDealerDebtsQuery request, CancellationToken cancellationToken)
    {
        var debts = request.DealerId.HasValue
            ? await _unitOfWork.DealerDebts.GetByDealerIdAsync(request.DealerId.Value)
            : await _unitOfWork.DealerDebts.GetAllAsync();

        var dealers = await _unitOfWork.Dealers.GetAllAsync();
        var dealerDict = dealers.ToDictionary(d => d.Id, d => d.Name);

        var result = debts.Select(d =>
        {
            var dto = _mapper.Map<DealerDebtDto>(d);
            dto.DealerName = dealerDict.TryGetValue(d.DealerId, out var name) ? name : string.Empty;
            return dto;
        }).OrderByDescending(d => d.CreatedAt).ToList();

        return result;
    }
}


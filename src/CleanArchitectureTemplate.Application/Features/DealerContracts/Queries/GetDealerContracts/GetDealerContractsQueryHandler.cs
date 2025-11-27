using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerContracts.Queries.GetDealerContracts;

public class GetDealerContractsQueryHandler : IRequestHandler<GetDealerContractsQuery, List<DealerContractDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerContractsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<DealerContractDto>> Handle(GetDealerContractsQuery request, CancellationToken cancellationToken)
    {
        var contracts = request.DealerId.HasValue
            ? await _unitOfWork.DealerContracts.GetByDealerIdAsync(request.DealerId.Value)
            : await _unitOfWork.DealerContracts.GetAllAsync();

        if (request.Status.HasValue)
        {
            contracts = contracts.Where(c => c.Status == request.Status.Value).ToList();
        }

        var dealers = await _unitOfWork.Dealers.GetAllAsync();
        var dealerDict = dealers.ToDictionary(d => d.Id, d => d.Name);

        var result = contracts.Select(c =>
        {
            var dto = _mapper.Map<DealerContractDto>(c);
            dto.DealerName = dealerDict.TryGetValue(c.DealerId, out var name) ? name : string.Empty;
            return dto;
        }).OrderByDescending(c => c.CreatedAt).ToList();

        return result;
    }
}


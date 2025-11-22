using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Queries.GetQuotations;

public class GetQuotationsQueryHandler : IRequestHandler<GetQuotationsQuery, List<QuotationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuotationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<QuotationDto>> Handle(GetQuotationsQuery request, CancellationToken cancellationToken)
    {
        var quotations = request.DealerId.HasValue
            ? await _unitOfWork.Quotations.GetByDealerIdAsync(request.DealerId.Value)
            : request.CustomerId.HasValue
                ? await _unitOfWork.Quotations.GetByCustomerIdAsync(request.CustomerId.Value)
                : await _unitOfWork.Quotations.GetAllAsync();

        return _mapper.Map<List<QuotationDto>>(quotations);
    }
}


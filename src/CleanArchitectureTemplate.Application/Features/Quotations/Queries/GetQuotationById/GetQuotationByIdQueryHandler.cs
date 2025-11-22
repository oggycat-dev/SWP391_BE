using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Queries.GetQuotationById;

public class GetQuotationByIdQueryHandler : IRequestHandler<GetQuotationByIdQuery, QuotationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuotationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuotationDto> Handle(GetQuotationByIdQuery request, CancellationToken cancellationToken)
    {
        var quotation = await _unitOfWork.Quotations.GetByIdWithDetailsAsync(request.Id);
        if (quotation == null)
        {
            throw new NotFoundException("Quotation", request.Id);
        }

        return _mapper.Map<QuotationDto>(quotation);
    }
}


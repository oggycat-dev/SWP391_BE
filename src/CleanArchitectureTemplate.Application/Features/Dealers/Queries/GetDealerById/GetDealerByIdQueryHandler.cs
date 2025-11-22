using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Queries.GetDealerById;

public class GetDealerByIdQueryHandler : IRequestHandler<GetDealerByIdQuery, DealerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDto> Handle(GetDealerByIdQuery request, CancellationToken cancellationToken)
    {
        var dealer = await _unitOfWork.Dealers.GetByIdWithDetailsAsync(request.Id)
            ?? throw new NotFoundException($"Dealer with ID {request.Id} not found");

        return _mapper.Map<DealerDto>(dealer);
    }
}

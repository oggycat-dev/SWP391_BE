using AutoMapper;
using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;

namespace CleanArchitectureTemplate.Application.Features.DealerDiscountPolicies.Queries.GetDealerDiscountPolicyById;

public class GetDealerDiscountPolicyByIdQueryHandler : IRequestHandler<GetDealerDiscountPolicyByIdQuery, DealerDiscountPolicyDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerDiscountPolicyByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDiscountPolicyDto> Handle(GetDealerDiscountPolicyByIdQuery request, CancellationToken cancellationToken)
    {
        var policy = await _unitOfWork.DealerDiscountPolicies.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Dealer discount policy with ID {request.Id} not found");

        var dto = _mapper.Map<DealerDiscountPolicyDto>(policy);
        
        // Load dealer name
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(policy.DealerId);
        dto.DealerName = dealer?.Name ?? string.Empty;

        return dto;
    }
}


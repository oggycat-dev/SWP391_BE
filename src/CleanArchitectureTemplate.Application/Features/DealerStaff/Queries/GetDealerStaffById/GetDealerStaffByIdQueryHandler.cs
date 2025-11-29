using AutoMapper;
using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Queries.GetDealerStaffById;

public class GetDealerStaffByIdQueryHandler : IRequestHandler<GetDealerStaffByIdQuery, DealerStaffDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerStaffByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerStaffDto> Handle(GetDealerStaffByIdQuery request, CancellationToken cancellationToken)
    {
        var dealerStaff = await _unitOfWork.DealerStaff.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Dealer staff with ID {request.Id} not found");

        var dto = _mapper.Map<DealerStaffDto>(dealerStaff);
        
        // Load dealer name
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(dealerStaff.DealerId);
        dto.DealerName = dealer?.Name ?? string.Empty;
        
        // Load user name
        var user = await _unitOfWork.Users.GetByIdAsync(dealerStaff.UserId);
        dto.UserName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty;

        return dto;
    }
}


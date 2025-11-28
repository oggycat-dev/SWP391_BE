using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.CreateDealerStaff;

public class CreateDealerStaffCommandHandler : IRequestHandler<CreateDealerStaffCommand, DealerStaffDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDealerStaffCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerStaffDto> Handle(CreateDealerStaffCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found");
        }

        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId);
        if (dealer == null)
        {
            throw new KeyNotFoundException($"Dealer with ID {request.DealerId} not found");
        }

        // Check if user is already a staff member of this dealer
        var existingStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(request.UserId);
        if (existingStaff != null && existingStaff.DealerId == request.DealerId && existingStaff.IsActive)
        {
            throw new InvalidOperationException("User is already a staff member of this dealer");
        }

        var staff = new Domain.Entities.DealerStaff
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            DealerId = request.DealerId,
            Position = request.Position,
            SalesTarget = request.SalesTarget,
            CurrentSales = 0,
            CommissionRate = request.CommissionRate,
            JoinedDate = request.JoinedDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DealerStaff.AddAsync(staff);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<DealerStaffDto>(staff);
        result.UserName = $"{user.FirstName} {user.LastName}";
        result.UserEmail = user.Email;
        result.DealerName = dealer.Name;
        
        return result;
    }
}


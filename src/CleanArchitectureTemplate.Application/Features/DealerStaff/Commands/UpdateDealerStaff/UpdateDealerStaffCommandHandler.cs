using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.UpdateDealerStaff;

public class UpdateDealerStaffCommandHandler : IRequestHandler<UpdateDealerStaffCommand, DealerStaffDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDealerStaffCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerStaffDto> Handle(UpdateDealerStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await _unitOfWork.DealerStaff.GetByIdWithDetailsAsync(request.Id);
        if (staff == null)
        {
            throw new KeyNotFoundException($"Dealer staff with ID {request.Id} not found");
        }

        staff.Position = request.Position;
        staff.SalesTarget = request.SalesTarget;
        staff.CommissionRate = request.CommissionRate;
        staff.IsActive = request.IsActive;
        
        if (!request.IsActive && staff.LeftDate == null)
        {
            staff.LeftDate = DateTime.UtcNow;
        }
        else if (request.IsActive)
        {
            staff.LeftDate = null;
        }

        staff.ModifiedAt = DateTime.UtcNow;

        await _unitOfWork.DealerStaff.UpdateAsync(staff);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<DealerStaffDto>(staff);
        result.UserName = $"{staff.User.FirstName} {staff.User.LastName}";
        result.UserEmail = staff.User.Email;
        result.DealerName = staff.Dealer.Name;
        
        return result;
    }
}


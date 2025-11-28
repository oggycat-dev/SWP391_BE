using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Queries.GetDealerStaff;

public class GetDealerStaffQueryHandler : IRequestHandler<GetDealerStaffQuery, List<DealerStaffDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDealerStaffQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<DealerStaffDto>> Handle(GetDealerStaffQuery request, CancellationToken cancellationToken)
    {
        var staffList = request.DealerId.HasValue
            ? await _unitOfWork.DealerStaff.GetByDealerIdAsync(request.DealerId.Value)
            : (await _unitOfWork.DealerStaff.GetAllAsync()).ToList();

        // Filter to get only non-deleted staff with details
        var result = new List<DealerStaffDto>();
        foreach (var staff in staffList)
        {
            var staffWithDetails = await _unitOfWork.DealerStaff.GetByIdWithDetailsAsync(staff.Id);
            if (staffWithDetails != null)
            {
                var dto = _mapper.Map<DealerStaffDto>(staffWithDetails);
                dto.UserName = $"{staffWithDetails.User.FirstName} {staffWithDetails.User.LastName}";
                dto.UserEmail = staffWithDetails.User.Email;
                dto.DealerName = staffWithDetails.Dealer.Name;
                result.Add(dto);
            }
        }

        return result.OrderByDescending(s => s.IsActive).ThenBy(s => s.UserName).ToList();
    }
}


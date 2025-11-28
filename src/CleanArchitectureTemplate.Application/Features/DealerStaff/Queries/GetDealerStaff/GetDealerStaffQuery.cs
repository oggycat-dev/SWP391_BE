using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Queries.GetDealerStaff;

public class GetDealerStaffQuery : IRequest<List<DealerStaffDto>>
{
    public Guid? DealerId { get; set; }
}


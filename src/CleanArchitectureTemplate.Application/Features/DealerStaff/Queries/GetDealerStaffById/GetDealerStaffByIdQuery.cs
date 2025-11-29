using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Queries.GetDealerStaffById;

public class GetDealerStaffByIdQuery : IRequest<DealerStaffDto>
{
    public Guid Id { get; set; }
}


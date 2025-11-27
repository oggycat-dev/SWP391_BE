using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.CreateDealerStaff;

public class CreateDealerStaffCommand : IRequest<DealerStaffDto>
{
    public Guid UserId { get; set; }
    public Guid DealerId { get; set; }
    public string Position { get; set; } = string.Empty;
    public decimal SalesTarget { get; set; }
    public decimal CommissionRate { get; set; }
    public DateTime JoinedDate { get; set; }
}


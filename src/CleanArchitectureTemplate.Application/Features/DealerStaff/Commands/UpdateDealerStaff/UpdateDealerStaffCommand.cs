using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.UpdateDealerStaff;

public class UpdateDealerStaffCommand : IRequest<DealerStaffDto>
{
    public Guid Id { get; set; }
    public string Position { get; set; } = string.Empty;
    public decimal SalesTarget { get; set; }
    public decimal CommissionRate { get; set; }
    public bool IsActive { get; set; }
}


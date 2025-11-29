using MediatR;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.DeleteDealerStaff;

/// <summary>
/// Delete dealer staff command
/// </summary>
public class DeleteDealerStaffCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}


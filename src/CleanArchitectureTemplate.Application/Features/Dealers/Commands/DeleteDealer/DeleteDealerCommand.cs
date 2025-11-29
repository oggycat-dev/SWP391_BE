using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.DeleteDealer;

/// <summary>
/// Delete (soft delete) dealer command
/// </summary>
public class DeleteDealerCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}


using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.DeletePromotion;

public class DeletePromotionCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}


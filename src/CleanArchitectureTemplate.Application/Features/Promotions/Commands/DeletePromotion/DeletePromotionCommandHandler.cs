using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.DeletePromotion;

public class DeletePromotionCommandHandler : IRequestHandler<DeletePromotionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePromotionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _unitOfWork.Promotions.GetByIdAsync(request.Id);
        if (promotion == null)
        {
            throw new NotFoundException(nameof(Promotion), request.Id);
        }

        await _unitOfWork.Promotions.DeleteAsync(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


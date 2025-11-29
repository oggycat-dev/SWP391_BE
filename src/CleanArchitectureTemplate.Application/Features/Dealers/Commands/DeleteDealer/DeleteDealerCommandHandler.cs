using MediatR;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.DeleteDealer;

public class DeleteDealerCommandHandler : IRequestHandler<DeleteDealerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDealerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDealerCommand request, CancellationToken cancellationToken)
    {
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Dealer with ID {request.Id} not found");

        // Soft delete: Change status to Inactive
        dealer.Status = DealerStatus.Inactive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}


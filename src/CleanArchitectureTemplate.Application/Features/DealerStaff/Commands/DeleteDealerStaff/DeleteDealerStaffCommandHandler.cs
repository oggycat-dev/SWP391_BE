using MediatR;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;

namespace CleanArchitectureTemplate.Application.Features.DealerStaff.Commands.DeleteDealerStaff;

public class DeleteDealerStaffCommandHandler : IRequestHandler<DeleteDealerStaffCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDealerStaffCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDealerStaffCommand request, CancellationToken cancellationToken)
    {
        var dealerStaff = await _unitOfWork.DealerStaff.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Dealer staff with ID {request.Id} not found");

        // Hard delete - remove staff from dealer
        await _unitOfWork.DealerStaff.DeleteAsync(dealerStaff);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}


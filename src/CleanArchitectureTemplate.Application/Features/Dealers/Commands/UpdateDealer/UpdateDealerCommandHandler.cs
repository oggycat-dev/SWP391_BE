using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.UpdateDealer;

public class UpdateDealerCommandHandler : IRequestHandler<UpdateDealerCommand, DealerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDealerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDto> Handle(UpdateDealerCommand request, CancellationToken cancellationToken)
    {
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Dealer with ID {request.Id} not found");

        // Validate unique email if changed
        if (dealer.Email != request.Email)
        {
            var existingDealer = await _unitOfWork.Dealers.FirstOrDefaultAsync(d => 
                d.Email == request.Email && d.Id != request.Id);

            if (existingDealer != null)
            {
                throw new ValidationException("Email already exists");
            }
        }

        // Update dealer properties
        dealer.Name = request.DealerName;
        dealer.Address = request.Address;
        dealer.City = request.City;
        dealer.Region = request.District;
        dealer.PhoneNumber = request.PhoneNumber;
        dealer.Email = request.Email;
        dealer.DebtLimit = request.DebtLimit;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DealerDto>(dealer);
    }
}


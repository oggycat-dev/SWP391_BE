using MediatR;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.CreateDealer;

public class CreateDealerCommandHandler : IRequestHandler<CreateDealerCommand, DealerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDealerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DealerDto> Handle(CreateDealerCommand request, CancellationToken cancellationToken)
    {
        var existingDealer = await _unitOfWork.Dealers.GetByDealerCodeAsync(request.DealerCode);
        if (existingDealer != null)
        {
            throw new ValidationException("Dealer with this code already exists");
        }

        var dealer = new Dealer
        {
            Id = Guid.NewGuid(),
            DealerCode = request.DealerCode,
            Name = request.DealerName,
            Address = request.Address,
            City = request.City,
            Region = request.District,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Status = DealerStatus.Active,
            DebtLimit = request.DebtLimit,
            CurrentDebt = 0
        };

        await _unitOfWork.Dealers.AddAsync(dealer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DealerDto>(dealer);
    }
}

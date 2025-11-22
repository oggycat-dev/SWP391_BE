using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Get current dealer staff
        var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
        if (dealerStaff == null)
        {
            throw new UnauthorizedAccessException("User is not associated with any dealer.");
        }

        // Generate customer code
        var customerCode = await GenerateCustomerCodeAsync();

        var customer = new Customer
        {
            CustomerCode = customerCode,
            FullName = request.FullName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            IdNumber = request.IdNumber,
            DateOfBirth = request.DateOfBirth,
            Notes = request.Notes,
            CreatedByDealerId = dealerStaff.DealerId
        };

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerDto>(customer);
    }

    private async Task<string> GenerateCustomerCodeAsync()
    {
        var count = (await _unitOfWork.Customers.GetAllAsync()).Count;
        return $"CUS{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


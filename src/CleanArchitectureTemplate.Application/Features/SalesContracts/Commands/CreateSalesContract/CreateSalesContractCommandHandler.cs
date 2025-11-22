using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Sales;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.SalesContracts.Commands.CreateSalesContract;

public class CreateSalesContractCommandHandler : IRequestHandler<CreateSalesContractCommand, SalesContractDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateSalesContractCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<SalesContractDto> Handle(CreateSalesContractCommand request, CancellationToken cancellationToken)
    {
        // Validate order exists
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(request.OrderId);
        if (order == null)
        {
            throw new NotFoundException("Order", request.OrderId);
        }

        // Check if order is approved
        if (order.Status != OrderStatus.Approved)
        {
            throw new Common.Exceptions.ValidationException("Only approved orders can have sales contracts");
        }

        // Check if contract already exists
        var existingContract = await _unitOfWork.SalesContracts.GetByOrderIdAsync(request.OrderId);
        if (existingContract != null)
        {
            throw new Common.Exceptions.ValidationException("Sales contract already exists for this order");
        }

        // Generate contract number
        var contractNumber = await GenerateContractNumber();

        var contract = new SalesContract
        {
            Id = Guid.NewGuid(),
            ContractNumber = contractNumber,
            OrderId = request.OrderId,
            SignedDate = request.ContractDate,
            Terms = request.SpecialTerms ?? string.Empty,
            TotalAmount = order.TotalAmount,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        await _unitOfWork.SalesContracts.AddAsync(contract);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SalesContractDto>(contract);
    }

    private async Task<string> GenerateContractNumber()
    {
        var contracts = await _unitOfWork.SalesContracts.GetAllAsync();
        var count = contracts.Count + 1;
        return $"SC-{DateTime.UtcNow:yyyyMMdd}-{count:D5}";
    }
}


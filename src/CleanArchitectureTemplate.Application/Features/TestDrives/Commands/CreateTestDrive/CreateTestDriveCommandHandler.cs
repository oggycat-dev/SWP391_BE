using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Commands.CreateTestDrive;

public class CreateTestDriveCommandHandler : IRequestHandler<CreateTestDriveCommand, TestDriveDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateTestDriveCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<TestDriveDto> Handle(CreateTestDriveCommand request, CancellationToken cancellationToken)
    {
        // Validate customer
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException("Customer", request.CustomerId);
        }

        // Validate vehicle variant
        var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VehicleVariantId);
        if (variant == null)
        {
            throw new NotFoundException("Vehicle Variant", request.VehicleVariantId);
        }

        // Validate dealer
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId);
        if (dealer == null)
        {
            throw new NotFoundException("Dealer", request.DealerId);
        }

        var testDrive = new TestDrive
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            VehicleVariantId = request.VehicleVariantId,
            DealerId = request.DealerId,
            ScheduledDate = request.ScheduledDate,
            Status = TestDriveStatus.Scheduled,
            Notes = request.Notes ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        await _unitOfWork.TestDrives.AddAsync(testDrive);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TestDriveDto>(testDrive);
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Commands.UpdateTestDriveStatus;

public class UpdateTestDriveStatusCommandHandler : IRequestHandler<UpdateTestDriveStatusCommand, TestDriveDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateTestDriveStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<TestDriveDto> Handle(UpdateTestDriveStatusCommand request, CancellationToken cancellationToken)
    {
        var testDrive = await _unitOfWork.TestDrives.GetByIdAsync(request.Id);
        if (testDrive == null)
        {
            throw new NotFoundException("Test Drive", request.Id);
        }

        if (!Enum.TryParse<TestDriveStatus>(request.Status, out var status))
        {
            throw new Common.Exceptions.ValidationException($"Invalid test drive status: {request.Status}");
        }

        testDrive.Status = status;
        testDrive.Feedback = request.Feedback ?? string.Empty;
        testDrive.Rating = request.Rating;
        testDrive.ModifiedAt = DateTime.UtcNow;
        testDrive.ModifiedBy = _currentUserService.UserId;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TestDriveDto>(testDrive);
    }
}


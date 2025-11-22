using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Commands.UpdateTestDriveStatus;

public class UpdateTestDriveStatusCommand : IRequest<TestDriveDto>
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Feedback { get; set; }
    public int? Rating { get; set; }
}


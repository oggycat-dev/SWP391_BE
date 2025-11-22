using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Commands.CreateTestDrive;

public class CreateTestDriveCommand : IRequest<TestDriveDto>
{
    public Guid CustomerId { get; set; }
    public Guid VehicleVariantId { get; set; }
    public Guid DealerId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string? Notes { get; set; }
}


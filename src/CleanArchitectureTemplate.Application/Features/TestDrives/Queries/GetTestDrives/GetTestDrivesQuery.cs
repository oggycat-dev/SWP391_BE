using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using CleanArchitectureTemplate.Application.Common.Models;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Queries.GetTestDrives;

public class GetTestDrivesQuery : IRequest<PaginatedResult<TestDriveDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CustomerId { get; set; }
    public Guid? DealerId { get; set; }
    public string? Status { get; set; }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.TestDrives.Queries.GetTestDrives;

public class GetTestDrivesQueryHandler : IRequestHandler<GetTestDrivesQuery, PaginatedResult<TestDriveDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTestDrivesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<TestDriveDto>> Handle(GetTestDrivesQuery request, CancellationToken cancellationToken)
    {
        var testDrives = await _unitOfWork.TestDrives.GetAllAsync();

        if (request.CustomerId.HasValue)
        {
            testDrives = testDrives.Where(t => t.CustomerId == request.CustomerId.Value).ToList();
        }

        if (request.DealerId.HasValue)
        {
            testDrives = testDrives.Where(t => t.DealerId == request.DealerId.Value).ToList();
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            if (Enum.TryParse<TestDriveStatus>(request.Status, out var status))
            {
                testDrives = testDrives.Where(t => t.Status == status).ToList();
            }
        }

        var totalCount = testDrives.Count;
        var items = testDrives
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PaginatedResult<TestDriveDto>
        {
            Items = _mapper.Map<List<TestDriveDto>>(items),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}


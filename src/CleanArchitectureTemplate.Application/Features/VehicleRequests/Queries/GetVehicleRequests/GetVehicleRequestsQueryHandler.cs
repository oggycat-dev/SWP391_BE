using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleRequests.Queries.GetVehicleRequests;

public class GetVehicleRequestsQueryHandler : IRequestHandler<GetVehicleRequestsQuery, List<VehicleRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetVehicleRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<VehicleRequestDto>> Handle(GetVehicleRequestsQuery request, CancellationToken cancellationToken)
    {
        var vehicleRequests = request.PendingOnly
            ? await _unitOfWork.VehicleRequests.GetPendingRequestsAsync()
            : request.DealerId.HasValue
                ? await _unitOfWork.VehicleRequests.GetByDealerIdAsync(request.DealerId.Value)
                : await _unitOfWork.VehicleRequests.GetAllAsync();

        if (request.Status.HasValue && !request.PendingOnly)
        {
            vehicleRequests = vehicleRequests.Where(vr => vr.Status == request.Status.Value).ToList();
        }

        return _mapper.Map<List<VehicleRequestDto>>(vehicleRequests);
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Queries.GetDeliveries;

public class GetDeliveriesQueryHandler : IRequestHandler<GetDeliveriesQuery, List<DeliveryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetDeliveriesQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<DeliveryDto>> Handle(GetDeliveriesQuery request, CancellationToken cancellationToken)
    {
        var deliveries = await _unitOfWork.Deliveries.GetAllAsync();

        // Filter by dealer if provided or if user is dealer staff
        if (request.DealerId.HasValue)
        {
            var deliveriesForDealer = await _unitOfWork.Deliveries.GetByDealerIdAsync(request.DealerId.Value);
            deliveries = deliveriesForDealer;
        }
        else
        {
            // Check if current user is dealer staff
            var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
            if (dealerStaff != null)
            {
                var deliveriesForDealer = await _unitOfWork.Deliveries.GetByDealerIdAsync(dealerStaff.DealerId);
                deliveries = deliveriesForDealer;
            }
        }

        // Apply filters
        var query = deliveries.AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(d => d.Status == request.Status.Value);
        }

        if (request.FromDate.HasValue)
        {
            query = query.Where(d => d.ScheduledDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            query = query.Where(d => d.ScheduledDate <= request.ToDate.Value);
        }

        var result = query.OrderByDescending(d => d.ScheduledDate).ToList();

        return _mapper.Map<List<DeliveryDto>>(result);
    }
}


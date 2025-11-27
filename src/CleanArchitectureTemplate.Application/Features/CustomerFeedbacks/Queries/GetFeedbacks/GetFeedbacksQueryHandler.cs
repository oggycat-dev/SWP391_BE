using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbacks;

public class GetFeedbacksQueryHandler : IRequestHandler<GetFeedbacksQuery, List<CustomerFeedbackDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetFeedbacksQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<CustomerFeedbackDto>> Handle(GetFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var feedbacks = await _unitOfWork.CustomerFeedbacks.GetAllAsync();

        // Filter by dealer if provided or if user is dealer staff
        if (request.DealerId.HasValue)
        {
            feedbacks = await _unitOfWork.CustomerFeedbacks.GetByDealerIdAsync(request.DealerId.Value);
        }
        else if (request.CustomerId.HasValue)
        {
            feedbacks = await _unitOfWork.CustomerFeedbacks.GetByCustomerIdAsync(request.CustomerId.Value);
        }
        else
        {
            // Check if current user is dealer staff
            var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
            if (dealerStaff != null)
            {
                feedbacks = await _unitOfWork.CustomerFeedbacks.GetByDealerIdAsync(dealerStaff.DealerId);
            }
        }

        // Apply filters
        var query = feedbacks.AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(f => f.FeedbackStatus == request.Status.Value);
        }

        if (request.MinRating.HasValue)
        {
            query = query.Where(f => f.Rating >= request.MinRating.Value);
        }

        if (request.MaxRating.HasValue)
        {
            query = query.Where(f => f.Rating <= request.MaxRating.Value);
        }

        var result = query.OrderByDescending(f => f.CreatedAt).ToList();

        return _mapper.Map<List<CustomerFeedbackDto>>(result);
    }
}


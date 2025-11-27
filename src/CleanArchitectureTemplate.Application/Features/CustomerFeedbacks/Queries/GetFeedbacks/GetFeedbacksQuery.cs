using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbacks;

public class GetFeedbacksQuery : IRequest<List<CustomerFeedbackDto>>
{
    public Guid? DealerId { get; set; }
    public Guid? CustomerId { get; set; }
    public FeedbackStatus? Status { get; set; }
    public int? MinRating { get; set; }
    public int? MaxRating { get; set; }
}


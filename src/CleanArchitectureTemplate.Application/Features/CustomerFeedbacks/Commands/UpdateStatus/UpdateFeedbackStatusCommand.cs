using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.UpdateStatus;

public class UpdateFeedbackStatusCommand : IRequest<CustomerFeedbackDto>
{
    public Guid Id { get; set; }
    public FeedbackStatus? FeedbackStatus { get; set; }
    public ComplaintStatus? ComplaintStatus { get; set; }
}


using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.SubmitFeedback;

public class SubmitFeedbackCommand : IRequest<CustomerFeedbackDto>
{
    public Guid CustomerId { get; set; }
    public Guid? OrderId { get; set; }
    public Guid DealerId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public bool IsComplaint { get; set; }
}


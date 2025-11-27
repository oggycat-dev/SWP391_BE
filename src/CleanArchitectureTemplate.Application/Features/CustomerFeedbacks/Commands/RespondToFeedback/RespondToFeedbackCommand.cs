using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.RespondToFeedback;

public class RespondToFeedbackCommand : IRequest<CustomerFeedbackDto>
{
    public Guid Id { get; set; }
    public string Response { get; set; } = string.Empty;
}


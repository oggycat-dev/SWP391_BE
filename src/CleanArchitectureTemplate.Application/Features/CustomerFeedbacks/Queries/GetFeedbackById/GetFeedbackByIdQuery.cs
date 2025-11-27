using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbackById;

public class GetFeedbackByIdQuery : IRequest<CustomerFeedbackDto>
{
    public Guid Id { get; set; }
}


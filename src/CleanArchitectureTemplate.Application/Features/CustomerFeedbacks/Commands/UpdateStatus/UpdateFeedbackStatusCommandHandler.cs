using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.UpdateStatus;

public class UpdateFeedbackStatusCommandHandler : IRequestHandler<UpdateFeedbackStatusCommand, CustomerFeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateFeedbackStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerFeedbackDto> Handle(UpdateFeedbackStatusCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.CustomerFeedbacks.GetByIdAsync(request.Id);
        if (feedback == null)
        {
            throw new NotFoundException(nameof(CustomerFeedback), request.Id);
        }

        if (request.FeedbackStatus.HasValue)
        {
            feedback.FeedbackStatus = request.FeedbackStatus.Value;
        }

        if (request.ComplaintStatus.HasValue)
        {
            feedback.ComplaintStatus = request.ComplaintStatus.Value;
        }

        await _unitOfWork.CustomerFeedbacks.UpdateAsync(feedback);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details
        var feedbackWithDetails = await _unitOfWork.CustomerFeedbacks.GetByIdWithDetailsAsync(feedback.Id);
        
        return _mapper.Map<CustomerFeedbackDto>(feedbackWithDetails);
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.RespondToFeedback;

public class RespondToFeedbackCommandHandler : IRequestHandler<RespondToFeedbackCommand, CustomerFeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public RespondToFeedbackCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CustomerFeedbackDto> Handle(RespondToFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.CustomerFeedbacks.GetByIdAsync(request.Id);
        if (feedback == null)
        {
            throw new NotFoundException(nameof(CustomerFeedback), request.Id);
        }

        if (string.IsNullOrEmpty(request.Response))
        {
            throw new ValidationException("Response is required");
        }

        feedback.Response = request.Response;
        feedback.ResponseDate = DateTime.UtcNow;
        feedback.RespondedBy = _currentUserService.UserId;
        feedback.FeedbackStatus = FeedbackStatus.InProgress;

        if (feedback.ComplaintStatus == ComplaintStatus.Open)
        {
            feedback.ComplaintStatus = ComplaintStatus.Investigating;
        }

        await _unitOfWork.CustomerFeedbacks.UpdateAsync(feedback);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details
        var feedbackWithDetails = await _unitOfWork.CustomerFeedbacks.GetByIdWithDetailsAsync(feedback.Id);
        
        return _mapper.Map<CustomerFeedbackDto>(feedbackWithDetails);
    }
}


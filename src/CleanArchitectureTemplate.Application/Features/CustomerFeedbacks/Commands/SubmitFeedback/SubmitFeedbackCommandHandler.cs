using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Commands.SubmitFeedback;

public class SubmitFeedbackCommandHandler : IRequestHandler<SubmitFeedbackCommand, CustomerFeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubmitFeedbackCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerFeedbackDto> Handle(SubmitFeedbackCommand request, CancellationToken cancellationToken)
    {
        // Verify customer exists
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException(nameof(Customer), request.CustomerId);
        }

        // Verify dealer exists
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(request.DealerId);
        if (dealer == null)
        {
            throw new NotFoundException(nameof(Dealer), request.DealerId);
        }

        // Verify order if provided
        if (request.OrderId.HasValue)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId.Value);
            if (order == null)
            {
                throw new NotFoundException(nameof(Order), request.OrderId.Value);
            }
        }

        var feedback = new CustomerFeedback
        {
            CustomerId = request.CustomerId,
            OrderId = request.OrderId,
            DealerId = request.DealerId,
            Subject = request.Subject,
            Content = request.Content,
            Rating = request.Rating,
            FeedbackStatus = FeedbackStatus.Open,
            ComplaintStatus = request.IsComplaint ? ComplaintStatus.Open : null
        };

        await _unitOfWork.CustomerFeedbacks.AddAsync(feedback);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details
        var feedbackWithDetails = await _unitOfWork.CustomerFeedbacks.GetByIdWithDetailsAsync(feedback.Id);
        
        return _mapper.Map<CustomerFeedbackDto>(feedbackWithDetails);
    }
}


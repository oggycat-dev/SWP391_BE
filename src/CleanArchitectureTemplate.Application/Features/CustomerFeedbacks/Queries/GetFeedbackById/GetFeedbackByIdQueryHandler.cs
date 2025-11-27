using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.CustomerFeedbacks;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.CustomerFeedbacks.Queries.GetFeedbackById;

public class GetFeedbackByIdQueryHandler : IRequestHandler<GetFeedbackByIdQuery, CustomerFeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFeedbackByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerFeedbackDto> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.CustomerFeedbacks.GetByIdWithDetailsAsync(request.Id);
        
        if (feedback == null)
        {
            throw new NotFoundException(nameof(CustomerFeedback), request.Id);
        }

        return _mapper.Map<CustomerFeedbackDto>(feedback);
    }
}


using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.UpdatePromotionStatus;

public class UpdatePromotionStatusCommandHandler : IRequestHandler<UpdatePromotionStatusCommand, PromotionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePromotionStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PromotionDto> Handle(UpdatePromotionStatusCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _unitOfWork.Promotions.GetByIdAsync(request.Id);
        if (promotion == null)
        {
            throw new NotFoundException(nameof(Promotion), request.Id);
        }

        promotion.Status = request.Status;

        await _unitOfWork.Promotions.UpdateAsync(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PromotionDto>(promotion);
    }
}


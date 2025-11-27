using System.Text.Json;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandHandler : IRequestHandler<UpdatePromotionCommand, PromotionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePromotionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PromotionDto> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _unitOfWork.Promotions.GetByIdAsync(request.Id);
        if (promotion == null)
        {
            throw new NotFoundException(nameof(Promotion), request.Id);
        }

        promotion.Name = request.Name;
        promotion.Description = request.Description;
        promotion.StartDate = request.StartDate;
        promotion.EndDate = request.EndDate;
        promotion.DiscountType = request.DiscountType;
        promotion.DiscountPercentage = request.DiscountPercentage;
        promotion.DiscountAmount = request.DiscountAmount;
        promotion.ApplicableVehicleVariantIds = JsonSerializer.Serialize(request.ApplicableVehicleVariantIds);
        promotion.ApplicableDealerIds = JsonSerializer.Serialize(request.ApplicableDealerIds);
        promotion.MaxUsageCount = request.MaxUsageCount;
        promotion.ImageUrl = request.ImageUrl;
        promotion.TermsAndConditions = request.TermsAndConditions;

        await _unitOfWork.Promotions.UpdateAsync(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PromotionDto>(promotion);
    }
}


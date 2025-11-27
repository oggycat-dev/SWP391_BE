using System.Text.Json;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, PromotionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePromotionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PromotionDto> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        // Generate promotion code
        var promotionCode = await GeneratePromotionCodeAsync();

        var promotion = new Promotion
        {
            PromotionCode = promotionCode,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            DiscountType = request.DiscountType,
            DiscountPercentage = request.DiscountPercentage,
            DiscountAmount = request.DiscountAmount,
            Status = PromotionStatus.Active,
            ApplicableVehicleVariantIds = JsonSerializer.Serialize(request.ApplicableVehicleVariantIds),
            ApplicableDealerIds = JsonSerializer.Serialize(request.ApplicableDealerIds),
            MaxUsageCount = request.MaxUsageCount,
            CurrentUsageCount = 0,
            ImageUrl = request.ImageUrl,
            TermsAndConditions = request.TermsAndConditions
        };

        await _unitOfWork.Promotions.AddAsync(promotion);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PromotionDto>(promotion);
    }

    private async Task<string> GeneratePromotionCodeAsync()
    {
        var count = (await _unitOfWork.Promotions.GetAllAsync()).Count;
        return $"PROMO{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


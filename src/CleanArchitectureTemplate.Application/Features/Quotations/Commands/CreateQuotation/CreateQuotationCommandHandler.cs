using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Quotations.Commands.CreateQuotation;

public class CreateQuotationCommandHandler : IRequestHandler<CreateQuotationCommand, QuotationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateQuotationCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<QuotationDto> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
    {
        // Get current dealer staff
        var dealerStaff = await _unitOfWork.DealerStaff.GetByUserIdAsync(_currentUserService.UserId!.Value);
        if (dealerStaff == null)
        {
            throw new UnauthorizedAccessException("User is not associated with any dealer.");
        }

        // Validate customer
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
        if (customer == null)
        {
            throw new NotFoundException("Customer", request.CustomerId);
        }

        // Validate vehicle variant
        var variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(request.VehicleVariantId);
        if (variant == null)
        {
            throw new NotFoundException("Vehicle Variant", request.VehicleVariantId);
        }

        // Validate vehicle color
        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.VehicleColorId);
        if (color == null || color.VariantId != request.VehicleVariantId)
        {
            throw new ValidationException("Invalid vehicle color for the selected variant.");
        }

        // Calculate pricing
        var basePrice = variant.Price;
        var colorPrice = color.AdditionalPrice;
        var promotionDiscount = 0m;

        // Apply promotions
        if (request.PromotionIds.Any())
        {
            var promotions = await _unitOfWork.Promotions.GetActivePromotionsAsync();
            foreach (var promotionId in request.PromotionIds)
            {
                var promotion = promotions.FirstOrDefault(p => p.Id == promotionId);
                if (promotion != null)
                {
                    if (promotion.DiscountType == DiscountType.Percentage)
                    {
                        promotionDiscount += (basePrice + colorPrice) * (promotion.DiscountPercentage / 100);
                    }
                    else
                    {
                        promotionDiscount += promotion.DiscountAmount;
                    }
                }
            }
        }

        var subtotal = basePrice + colorPrice - request.DealerDiscount - promotionDiscount;
        var taxAmount = subtotal * 0.10m; // 10% VAT
        var totalAmount = subtotal + taxAmount;

        // Generate quotation number
        var quotationNumber = await GenerateQuotationNumberAsync();

        var quotation = new Quotation
        {
            QuotationNumber = quotationNumber,
            CustomerId = request.CustomerId,
            DealerId = dealerStaff.DealerId,
            DealerStaffId = dealerStaff.Id,
            VehicleVariantId = request.VehicleVariantId,
            VehicleColorId = request.VehicleColorId,
            Status = QuotationStatus.Draft,
            QuotationDate = DateTime.UtcNow,
            ValidUntil = DateTime.UtcNow.AddDays(request.ValidityDays),
            BasePrice = basePrice,
            ColorPrice = colorPrice,
            DealerDiscount = request.DealerDiscount,
            PromotionDiscount = promotionDiscount,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            Notes = request.Notes
        };

        await _unitOfWork.Quotations.AddAsync(quotation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _unitOfWork.Quotations.GetByIdWithDetailsAsync(quotation.Id);
        return _mapper.Map<QuotationDto>(result);
    }

    private async Task<string> GenerateQuotationNumberAsync()
    {
        var count = (await _unitOfWork.Quotations.GetAllAsync()).Count;
        return $"QUO{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


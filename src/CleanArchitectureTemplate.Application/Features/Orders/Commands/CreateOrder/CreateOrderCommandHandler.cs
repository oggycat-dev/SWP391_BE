using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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

        // Check dealer debt limit
        var dealer = await _unitOfWork.Dealers.GetByIdAsync(dealerStaff.DealerId);
        if (dealer == null)
        {
            throw new NotFoundException("Dealer", dealerStaff.DealerId);
        }

        var totalDebt = await _unitOfWork.DealerDebts.GetTotalDebtByDealerAsync(dealerStaff.DealerId);
        
        if (totalDebt >= dealer.DebtLimit)
        {
            throw new ValidationException("Dealer has exceeded debt limit. Cannot create new order.");
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

        // Generate order number
        var orderNumber = await GenerateOrderNumberAsync();

        var order = new Order
        {
            OrderNumber = orderNumber,
            QuotationId = request.QuotationId,
            CustomerId = request.CustomerId,
            DealerId = dealerStaff.DealerId,
            DealerStaffId = dealerStaff.Id,
            VehicleVariantId = request.VehicleVariantId,
            VehicleColorId = request.VehicleColorId,
            Status = OrderStatus.Draft,
            OrderDate = DateTime.UtcNow,
            BasePrice = basePrice,
            ColorPrice = colorPrice,
            DealerDiscount = request.DealerDiscount,
            PromotionDiscount = promotionDiscount,
            TaxAmount = taxAmount,
            TotalAmount = totalAmount,
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = PaymentStatus.Pending,
            PaidAmount = 0,
            RemainingAmount = totalAmount,
            IsInstallment = request.IsInstallment,
            Notes = request.Notes
        };

        await _unitOfWork.Orders.AddAsync(order);

        // Create order promotions
        if (request.PromotionIds.Any())
        {
            foreach (var promotionId in request.PromotionIds)
            {
                var promotion = await _unitOfWork.Promotions.GetByIdAsync(promotionId);
                if (promotion != null)
                {
                    var orderPromotion = new OrderPromotion
                    {
                        OrderId = order.Id,
                        PromotionId = promotionId,
                        DiscountAmount = promotion.DiscountType == DiscountType.Percentage
                            ? (basePrice + colorPrice) * (promotion.DiscountPercentage / 100)
                            : promotion.DiscountAmount
                    };
                    // Note: This would require adding OrderPromotion to UnitOfWork
                }
            }
        }

        // Update quotation if linked
        if (request.QuotationId.HasValue)
        {
            var quotation = await _unitOfWork.Quotations.GetByIdAsync(request.QuotationId.Value);
            if (quotation != null)
            {
                quotation.Status = QuotationStatus.Converted;
                await _unitOfWork.Quotations.UpdateAsync(quotation);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = await _unitOfWork.Orders.GetByIdWithDetailsAsync(order.Id);
        return _mapper.Map<OrderDto>(result);
    }

    private async Task<string> GenerateOrderNumberAsync()
    {
        var count = (await _unitOfWork.Orders.GetAllAsync()).Count;
        return $"ORD{DateTime.UtcNow:yyyyMMdd}{(count + 1):D4}";
    }
}


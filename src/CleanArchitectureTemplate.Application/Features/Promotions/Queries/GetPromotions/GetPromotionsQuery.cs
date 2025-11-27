using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetPromotions;

public class GetPromotionsQuery : IRequest<List<PromotionDto>>
{
    public PromotionStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}


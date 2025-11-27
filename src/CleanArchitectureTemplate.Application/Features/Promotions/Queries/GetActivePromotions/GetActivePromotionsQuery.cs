using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetActivePromotions;

public class GetActivePromotionsQuery : IRequest<List<PromotionDto>>
{
}


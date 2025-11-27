using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Queries.GetPromotionById;

public class GetPromotionByIdQuery : IRequest<PromotionDto>
{
    public Guid Id { get; set; }
}


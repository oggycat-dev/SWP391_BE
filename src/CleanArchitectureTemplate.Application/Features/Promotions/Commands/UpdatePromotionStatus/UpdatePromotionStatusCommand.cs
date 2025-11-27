using CleanArchitectureTemplate.Application.Common.DTOs.Promotions;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Promotions.Commands.UpdatePromotionStatus;

public class UpdatePromotionStatusCommand : IRequest<PromotionDto>
{
    public Guid Id { get; set; }
    public PromotionStatus Status { get; set; }
}


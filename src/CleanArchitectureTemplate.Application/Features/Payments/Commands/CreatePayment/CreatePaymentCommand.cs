using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using CleanArchitectureTemplate.Domain.Enums;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Payments.Commands.CreatePayment;

public class CreatePaymentCommand : IRequest<PaymentDto>
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public string? TransactionReference { get; set; }
    public string Notes { get; set; } = string.Empty;
}


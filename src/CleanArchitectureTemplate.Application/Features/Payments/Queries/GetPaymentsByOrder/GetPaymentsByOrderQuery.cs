using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Payments.Queries.GetPaymentsByOrder;

public class GetPaymentsByOrderQuery : IRequest<PaymentSummaryDto>
{
    public Guid OrderId { get; set; }
}

public class PaymentSummaryDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public bool IsInstallment { get; set; }
    
    public List<PaymentDto> Payments { get; set; } = new();
    public InstallmentPlanDto? InstallmentPlan { get; set; }
}


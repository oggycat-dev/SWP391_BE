using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Payments.Commands.CreateInstallmentPlan;

public class CreateInstallmentPlanCommand : IRequest<InstallmentPlanDto>
{
    public Guid OrderId { get; set; }
    public decimal DownPayment { get; set; }
    public int TermMonths { get; set; }
    public decimal InterestRate { get; set; }
    public string BankName { get; set; } = string.Empty;
}


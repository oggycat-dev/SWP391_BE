using CleanArchitectureTemplate.Application.Common.DTOs.Sales;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.SalesContracts.Commands.CreateSalesContract;

public class CreateSalesContractCommand : IRequest<SalesContractDto>
{
    public Guid OrderId { get; set; }
    public DateTime ContractDate { get; set; }
    public string? SpecialTerms { get; set; }
}


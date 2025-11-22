using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.CreateDealer;

public class CreateDealerCommand : IRequest<DealerDto>
{
    public string DealerCode { get; set; } = string.Empty;
    public string DealerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal DebtLimit { get; set; }
}

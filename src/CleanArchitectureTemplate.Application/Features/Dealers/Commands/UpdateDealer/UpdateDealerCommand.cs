using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Commands.UpdateDealer;

/// <summary>
/// Update dealer command following CQRS pattern
/// </summary>
public class UpdateDealerCommand : IRequest<DealerDto>
{
    /// <summary>
    /// Dealer ID
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Dealer name
    /// </summary>
    public string DealerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Dealer address
    /// </summary>
    public string Address { get; set; } = string.Empty;
    
    /// <summary>
    /// City
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// District
    /// </summary>
    public string District { get; set; } = string.Empty;
    
    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Debt limit
    /// </summary>
    public decimal DebtLimit { get; set; }
}


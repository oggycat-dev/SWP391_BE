using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<CustomerDto>
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string IdNumber { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string Notes { get; set; } = string.Empty;
}


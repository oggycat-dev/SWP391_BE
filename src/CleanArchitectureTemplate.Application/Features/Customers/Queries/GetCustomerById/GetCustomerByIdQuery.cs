using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public Guid Id { get; set; }
}


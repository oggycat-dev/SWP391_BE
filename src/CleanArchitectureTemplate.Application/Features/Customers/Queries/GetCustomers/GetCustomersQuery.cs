using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
    public Guid? DealerId { get; set; }
}


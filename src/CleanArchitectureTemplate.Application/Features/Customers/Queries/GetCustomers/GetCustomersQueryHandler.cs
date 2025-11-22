using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = request.DealerId.HasValue
            ? await _unitOfWork.Customers.GetByDealerIdAsync(request.DealerId.Value)
            : await _unitOfWork.Customers.GetAllAsync();

        return _mapper.Map<List<CustomerDto>>(customers);
    }
}


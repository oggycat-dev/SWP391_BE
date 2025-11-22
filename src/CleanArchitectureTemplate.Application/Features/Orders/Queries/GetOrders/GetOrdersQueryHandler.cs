using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = request.DealerId.HasValue
            ? await _unitOfWork.Orders.GetByDealerIdAsync(request.DealerId.Value)
            : request.CustomerId.HasValue
                ? await _unitOfWork.Orders.GetByCustomerIdAsync(request.CustomerId.Value)
                : request.DealerStaffId.HasValue
                    ? await _unitOfWork.Orders.GetByDealerStaffIdAsync(request.DealerStaffId.Value)
                    : await _unitOfWork.Orders.GetAllAsync();

        if (request.Status.HasValue)
        {
            orders = orders.Where(o => o.Status == request.Status.Value).ToList();
        }

        return _mapper.Map<List<OrderDto>>(orders);
    }
}


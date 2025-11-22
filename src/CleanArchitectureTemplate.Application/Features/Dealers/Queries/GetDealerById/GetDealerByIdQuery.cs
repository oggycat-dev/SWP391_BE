using MediatR;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Queries.GetDealerById;

public class GetDealerByIdQuery : IRequest<DealerDto>
{
    public Guid Id { get; set; }
}

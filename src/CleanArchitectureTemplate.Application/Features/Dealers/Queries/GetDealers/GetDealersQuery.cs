using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;

namespace CleanArchitectureTemplate.Application.Features.Dealers.Queries.GetDealers;

public class GetDealersQuery : PaginatedQuery<DealerDto>
{
    public string? Status { get; set; }
    public string? City { get; set; }
}

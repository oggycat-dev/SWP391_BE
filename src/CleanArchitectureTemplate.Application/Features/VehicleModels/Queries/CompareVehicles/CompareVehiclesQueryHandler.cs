using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Queries.CompareVehicles;

public class CompareVehiclesQueryHandler : IRequestHandler<CompareVehiclesQuery, List<VehicleVariantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CompareVehiclesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<VehicleVariantDto>> Handle(CompareVehiclesQuery request, CancellationToken cancellationToken)
    {
        var variants = new List<Domain.Entities.VehicleVariant>();

        foreach (var variantId in request.VariantIds)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdWithModelAsync(variantId);
            if (variant != null)
            {
                variants.Add(variant);
            }
        }

        return _mapper.Map<List<VehicleVariantDto>>(variants);
    }
}


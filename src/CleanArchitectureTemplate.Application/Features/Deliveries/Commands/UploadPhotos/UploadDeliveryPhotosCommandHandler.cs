using System.Text.Json;
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.UploadPhotos;

public class UploadDeliveryPhotosCommandHandler : IRequestHandler<UploadDeliveryPhotosCommand, DeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UploadDeliveryPhotosCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeliveryDto> Handle(UploadDeliveryPhotosCommand request, CancellationToken cancellationToken)
    {
        var delivery = await _unitOfWork.Deliveries.GetByIdAsync(request.Id);
        if (delivery == null)
        {
            throw new NotFoundException(nameof(Delivery), request.Id);
        }

        // Parse existing photos
        var existingPhotos = string.IsNullOrEmpty(delivery.DeliveryPhotos)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(delivery.DeliveryPhotos) ?? new List<string>();

        // Add new photos
        existingPhotos.AddRange(request.PhotoUrls);

        // Serialize back to JSON
        delivery.DeliveryPhotos = JsonSerializer.Serialize(existingPhotos);

        await _unitOfWork.Deliveries.UpdateAsync(delivery);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details
        var deliveryWithDetails = await _unitOfWork.Deliveries.GetByIdWithDetailsAsync(delivery.Id);
        
        return _mapper.Map<DeliveryDto>(deliveryWithDetails);
    }
}


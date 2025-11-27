using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.UploadPhotos;

public class UploadDeliveryPhotosCommand : IRequest<DeliveryDto>
{
    public Guid Id { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
}


using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CaptureSignature;

public class CaptureSignatureCommand : IRequest<DeliveryDto>
{
    public Guid Id { get; set; }
    public string SignatureBase64 { get; set; } = string.Empty;
}


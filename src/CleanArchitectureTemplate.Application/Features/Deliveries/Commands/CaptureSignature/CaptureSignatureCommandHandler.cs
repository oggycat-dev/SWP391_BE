using AutoMapper;
using CleanArchitectureTemplate.Application.Common.DTOs.Deliveries;
using CleanArchitectureTemplate.Application.Common.Exceptions;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using MediatR;

namespace CleanArchitectureTemplate.Application.Features.Deliveries.Commands.CaptureSignature;

public class CaptureSignatureCommandHandler : IRequestHandler<CaptureSignatureCommand, DeliveryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CaptureSignatureCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DeliveryDto> Handle(CaptureSignatureCommand request, CancellationToken cancellationToken)
    {
        var delivery = await _unitOfWork.Deliveries.GetByIdAsync(request.Id);
        if (delivery == null)
        {
            throw new NotFoundException(nameof(Delivery), request.Id);
        }

        if (string.IsNullOrEmpty(request.SignatureBase64))
        {
            throw new ValidationException("Signature is required");
        }

        delivery.ReceiverSignature = request.SignatureBase64;

        await _unitOfWork.Deliveries.UpdateAsync(delivery);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with details
        var deliveryWithDetails = await _unitOfWork.Deliveries.GetByIdWithDetailsAsync(delivery.Id);
        
        return _mapper.Map<DeliveryDto>(deliveryWithDetails);
    }
}


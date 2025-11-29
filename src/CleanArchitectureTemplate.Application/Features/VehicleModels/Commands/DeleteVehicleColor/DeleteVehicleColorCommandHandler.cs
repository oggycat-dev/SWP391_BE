using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Exceptions;

namespace CleanArchitectureTemplate.Application.Features.VehicleModels.Commands.DeleteVehicleColor;

public class DeleteVehicleColorCommandHandler : IRequestHandler<DeleteVehicleColorCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceProvider _serviceProvider;

    public DeleteVehicleColorCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _serviceProvider = serviceProvider;
    }

    public async Task<Unit> Handle(DeleteVehicleColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.VehicleColors.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Vehicle color with ID {request.Id} not found");

        var imageUrl = color.ImageUrl;

        await _unitOfWork.VehicleColors.DeleteAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Delete image in background
        if (!string.IsNullOrEmpty(imageUrl))
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();
                    await fileService.DeleteFileAsync(imageUrl);
                }
                catch { /* Ignore deletion errors */ }
            });
        }

        return Unit.Value;
    }
}

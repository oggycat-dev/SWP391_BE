using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Application.Common.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
}

public interface IVehicleModelRepository : IRepository<VehicleModel>
{
    Task<VehicleModel?> GetByModelCodeAsync(string modelCode);
    Task<IEnumerable<VehicleModel>> GetActiveModelsAsync();
}

public interface IVehicleVariantRepository : IRepository<VehicleVariant>
{
    Task<VehicleVariant?> GetByVariantCodeAsync(string variantCode);
    Task<IEnumerable<VehicleVariant>> GetByModelIdAsync(Guid modelId);
    Task<VehicleVariant?> GetByIdWithModelAsync(Guid id);
    Task<List<VehicleVariant>> GetAllWithModelsAsync();
}

public interface IVehicleColorRepository : IRepository<VehicleColor>
{
    Task<IEnumerable<VehicleColor>> GetByVariantIdAsync(Guid variantId);
    Task<VehicleColor?> GetByColorCodeAsync(string colorCode);
    Task<VehicleColor?> GetByIdWithVariantAsync(Guid id);
    Task<List<VehicleColor>> GetAllWithVariantsAsync();
}

public interface IVehicleInventoryRepository : IRepository<VehicleInventory>
{
    Task<VehicleInventory?> GetByVINAsync(string vinNumber);
    Task<IEnumerable<VehicleInventory>> GetAvailableVehiclesAsync();
    Task<IEnumerable<VehicleInventory>> GetByDealerIdAsync(Guid dealerId);
    Task<IEnumerable<VehicleInventory>> GetByStatusAsync(string status);
    Task<VehicleInventory?> GetByIdWithDetailsAsync(Guid id);
    Task<List<VehicleInventory>> GetAllWithDetailsAsync();
}

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IVehicleModelRepository VehicleModels { get; }
    IVehicleVariantRepository VehicleVariants { get; }
    IVehicleColorRepository VehicleColors { get; }
    IVehicleInventoryRepository VehicleInventories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
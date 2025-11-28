using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;

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

public interface IDealerRepository : IRepository<Dealer>
{
    Task<Dealer?> GetByDealerCodeAsync(string dealerCode);
    Task<List<Dealer>> GetActiveDealersAsync();
    Task<Dealer?> GetByIdWithDetailsAsync(Guid id);
}

public interface IDealerStaffRepository : IRepository<DealerStaff>
{
    Task<List<DealerStaff>> GetByDealerIdAsync(Guid dealerId);
    Task<DealerStaff?> GetByUserIdAsync(Guid userId);
    Task<DealerStaff?> GetByIdWithDetailsAsync(Guid id);
}

public interface IDealerContractRepository : IRepository<DealerContract>
{
    Task<DealerContract?> GetByContractNumberAsync(string contractNumber);
    Task<List<DealerContract>> GetByDealerIdAsync(Guid dealerId);
    Task<DealerContract?> GetActiveDealerContractAsync(Guid dealerId);
}

public interface IDealerDebtRepository : IRepository<DealerDebt>
{
    Task<List<DealerDebt>> GetByDealerIdAsync(Guid dealerId);
    Task<List<DealerDebt>> GetOverdueDebtsAsync();
    Task<decimal> GetTotalDebtByDealerAsync(Guid dealerId);
}

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByCustomerCodeAsync(string customerCode);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
    Task<List<Customer>> GetByDealerIdAsync(Guid dealerId);
    Task<Customer?> GetByIdWithOrdersAsync(Guid id);
}

public interface IQuotationRepository : IRepository<Quotation>
{
    Task<Quotation?> GetByQuotationNumberAsync(string quotationNumber);
    Task<List<Quotation>> GetByDealerIdAsync(Guid dealerId);
    Task<List<Quotation>> GetByCustomerIdAsync(Guid customerId);
    Task<Quotation?> GetByIdWithDetailsAsync(Guid id);
}

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<List<Order>> GetByDealerIdAsync(Guid dealerId);
    Task<List<Order>> GetByCustomerIdAsync(Guid customerId);
    Task<Order?> GetByIdWithDetailsAsync(Guid id);
    Task<List<Order>> GetByDealerStaffIdAsync(Guid dealerStaffId);
}

public interface IPaymentRepository : IRepository<Payment>
{
    Task<List<Payment>> GetByOrderIdAsync(Guid orderId);
    Task<decimal> GetTotalPaidByOrderIdAsync(Guid orderId);
}

public interface IInstallmentPlanRepository : IRepository<InstallmentPlan>
{
    Task<InstallmentPlan?> GetByOrderIdAsync(Guid orderId);
    Task<List<InstallmentPlan>> GetOverdueInstallmentsAsync();
}

public interface IVehicleRequestRepository : IRepository<VehicleRequest>
{
    Task<VehicleRequest?> GetByRequestCodeAsync(string requestCode);
    Task<List<VehicleRequest>> GetByDealerIdAsync(Guid dealerId);
    Task<List<VehicleRequest>> GetPendingRequestsAsync();
}

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<Promotion?> GetByPromotionCodeAsync(string promotionCode);
    Task<List<Promotion>> GetActivePromotionsAsync();
}

public interface ITestDriveRepository : IRepository<TestDrive>
{
    Task<List<TestDrive>> GetByDealerIdAsync(Guid dealerId);
    Task<List<TestDrive>> GetByCustomerIdAsync(Guid customerId);
    Task<List<TestDrive>> GetUpcomingTestDrivesAsync(Guid dealerId);
}

public interface ISalesContractRepository : IRepository<SalesContract>
{
    Task<SalesContract?> GetByOrderIdAsync(Guid orderId);
    Task<SalesContract?> GetByContractNumberAsync(string contractNumber);
}

public interface IDeliveryRepository : IRepository<Delivery>
{
    Task<Delivery?> GetByDeliveryCodeAsync(string deliveryCode);
    Task<Delivery?> GetByOrderIdAsync(Guid orderId);
    Task<List<Delivery>> GetByDealerIdAsync(Guid dealerId);
    Task<Delivery?> GetByIdWithDetailsAsync(Guid id);
}

public interface ICustomerFeedbackRepository : IRepository<CustomerFeedback>
{
    Task<List<CustomerFeedback>> GetByCustomerIdAsync(Guid customerId);
    Task<List<CustomerFeedback>> GetByDealerIdAsync(Guid dealerId);
    Task<List<CustomerFeedback>> GetByOrderIdAsync(Guid orderId);
    Task<List<CustomerFeedback>> GetByStatusAsync(FeedbackStatus status);
    Task<CustomerFeedback?> GetByIdWithDetailsAsync(Guid id);
}

public interface IDealerDiscountPolicyRepository : IRepository<DealerDiscountPolicy>
{
    Task<List<DealerDiscountPolicy>> GetByDealerIdAsync(Guid dealerId);
    Task<List<DealerDiscountPolicy>> GetActivePolicesAsync();
    Task<List<DealerDiscountPolicy>> GetByVehicleVariantIdAsync(Guid vehicleVariantId);
}

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IVehicleModelRepository VehicleModels { get; }
    IVehicleVariantRepository VehicleVariants { get; }
    IVehicleColorRepository VehicleColors { get; }
    IVehicleInventoryRepository VehicleInventories { get; }
    IDealerRepository Dealers { get; }
    IDealerStaffRepository DealerStaff { get; }
    IDealerContractRepository DealerContracts { get; }
    IDealerDebtRepository DealerDebts { get; }
    IDealerDiscountPolicyRepository DealerDiscountPolicies { get; }
    ICustomerRepository Customers { get; }
    IQuotationRepository Quotations { get; }
    IOrderRepository Orders { get; }
    IPaymentRepository Payments { get; }
    IInstallmentPlanRepository InstallmentPlans { get; }
    IVehicleRequestRepository VehicleRequests { get; }
    IPromotionRepository Promotions { get; }
    ITestDriveRepository TestDrives { get; }
    ISalesContractRepository SalesContracts { get; }
    IDeliveryRepository Deliveries { get; }
    ICustomerFeedbackRepository CustomerFeedbacks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
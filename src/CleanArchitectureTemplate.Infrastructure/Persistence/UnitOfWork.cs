using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Infrastructure.Persistence.Repositories;

namespace CleanArchitectureTemplate.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _userRepository;
    private IVehicleModelRepository? _vehicleModelRepository;
    private IVehicleVariantRepository? _vehicleVariantRepository;
    private IVehicleColorRepository? _vehicleColorRepository;
    private IVehicleInventoryRepository? _vehicleInventoryRepository;
    private IDealerRepository? _dealerRepository;
    private IDealerStaffRepository? _dealerStaffRepository;
    private IDealerContractRepository? _dealerContractRepository;
    private IDealerDebtRepository? _dealerDebtRepository;
    private ICustomerRepository? _customerRepository;
    private IQuotationRepository? _quotationRepository;
    private IOrderRepository? _orderRepository;
    private IPaymentRepository? _paymentRepository;
    private IInstallmentPlanRepository? _installmentPlanRepository;
    private IVehicleRequestRepository? _vehicleRequestRepository;
    private IPromotionRepository? _promotionRepository;
    private ITestDriveRepository? _testDriveRepository;
    private ISalesContractRepository? _salesContractRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _userRepository ??= new UserRepository(_context);
    public IVehicleModelRepository VehicleModels => _vehicleModelRepository ??= new VehicleModelRepository(_context);
    public IVehicleVariantRepository VehicleVariants => _vehicleVariantRepository ??= new VehicleVariantRepository(_context);
    public IVehicleColorRepository VehicleColors => _vehicleColorRepository ??= new VehicleColorRepository(_context);
    public IVehicleInventoryRepository VehicleInventories => _vehicleInventoryRepository ??= new VehicleInventoryRepository(_context);
    public IDealerRepository Dealers => _dealerRepository ??= new DealerRepository(_context);
    public IDealerStaffRepository DealerStaff => _dealerStaffRepository ??= new DealerStaffRepository(_context);
    public IDealerContractRepository DealerContracts => _dealerContractRepository ??= new DealerContractRepository(_context);
    public IDealerDebtRepository DealerDebts => _dealerDebtRepository ??= new DealerDebtRepository(_context);
    public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
    public IQuotationRepository Quotations => _quotationRepository ??= new QuotationRepository(_context);
    public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
    public IPaymentRepository Payments => _paymentRepository ??= new PaymentRepository(_context);
    public IInstallmentPlanRepository InstallmentPlans => _installmentPlanRepository ??= new InstallmentPlanRepository(_context);
    public IVehicleRequestRepository VehicleRequests => _vehicleRequestRepository ??= new VehicleRequestRepository(_context);
    public IPromotionRepository Promotions => _promotionRepository ??= new PromotionRepository(_context);
    public ITestDriveRepository TestDrives => _testDriveRepository ??= new TestDriveRepository(_context);
    public ISalesContractRepository SalesContracts => _salesContractRepository ??= new SalesContractRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
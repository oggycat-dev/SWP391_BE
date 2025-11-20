using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Commons;

namespace CleanArchitectureTemplate.Infrastructure.Persistence;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Users DbSet
    /// </summary>
    public DbSet<User> Users => Set<User>();

    // Vehicle Management
    public DbSet<VehicleModel> VehicleModels => Set<VehicleModel>();
    public DbSet<VehicleVariant> VehicleVariants => Set<VehicleVariant>();
    public DbSet<VehicleColor> VehicleColors => Set<VehicleColor>();
    public DbSet<VehicleInventory> VehicleInventories => Set<VehicleInventory>();

    // Dealer Management
    public DbSet<Dealer> Dealers => Set<Dealer>();
    public DbSet<DealerContract> DealerContracts => Set<DealerContract>();
    public DbSet<DealerStaff> DealerStaffs => Set<DealerStaff>();
    public DbSet<DealerDebt> DealerDebts => Set<DealerDebt>();
    public DbSet<DealerDiscountPolicy> DealerDiscountPolicies => Set<DealerDiscountPolicy>();

    // Customer Management
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<TestDrive> TestDrives => Set<TestDrive>();
    public DbSet<CustomerFeedback> CustomerFeedbacks => Set<CustomerFeedback>();

    // Sales Management
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<SalesContract> SalesContracts => Set<SalesContract>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();

    // Financial Management
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<InstallmentPlan> InstallmentPlans => Set<InstallmentPlan>();
    public DbSet<CustomerDebt> CustomerDebts => Set<CustomerDebt>();

    // Promotion Management
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<OrderPromotion> OrderPromotions => Set<OrderPromotion>();

    // Vehicle Request
    public DbSet<VehicleRequest> VehicleRequests => Set<VehicleRequest>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure User entity
        builder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Role).HasConversion<string>();
            
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsDeleted);
        });

        // Vehicle Model
        builder.Entity<VehicleModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ModelCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ModelName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.ModelCode).IsUnique();
            entity.HasIndex(e => e.IsActive);
        });

        // Vehicle Variant
        builder.Entity<VehicleVariant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VariantCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.VariantName).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.VariantCode).IsUnique();
            entity.HasOne(e => e.Model).WithMany(m => m.Variants).HasForeignKey(e => e.ModelId);
        });

        // Vehicle Color
        builder.Entity<VehicleColor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ColorName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ColorCode).HasMaxLength(20);
            entity.HasOne(e => e.Variant).WithMany(v => v.Colors).HasForeignKey(e => e.VariantId);
        });

        // Vehicle Inventory
        builder.Entity<VehicleInventory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VINNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.VINNumber).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.DealerId, e.Status });
            
            entity.HasOne(e => e.Variant).WithMany().HasForeignKey(e => e.VariantId);
            entity.HasOne(e => e.Color).WithMany(c => c.Inventories).HasForeignKey(e => e.ColorId);
            entity.HasOne(e => e.Dealer).WithMany(d => d.Inventories).HasForeignKey(e => e.DealerId).OnDelete(DeleteBehavior.SetNull);
        });

        // Dealer
        builder.Entity<Dealer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DealerCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.DealerCode).IsUnique();
            entity.HasIndex(e => e.Status);
        });

        // Dealer Contract
        builder.Entity<DealerContract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ContractNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ContractNumber).IsUnique();
            entity.HasOne(e => e.Dealer).WithMany(d => d.Contracts).HasForeignKey(e => e.DealerId);
        });

        // Dealer Staff
        builder.Entity<DealerStaff>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.DealerId });
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Dealer).WithMany(d => d.Staff).HasForeignKey(e => e.DealerId);
        });

        // Customer
        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.CustomerCode).IsUnique();
            entity.HasIndex(e => e.PhoneNumber);
            entity.HasOne(e => e.CreatedByDealer).WithMany().HasForeignKey(e => e.CreatedByDealerId);
        });

        // Quotation
        builder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.QuotationNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.QuotationNumber).IsUnique();
            entity.HasIndex(e => new { e.DealerId, e.Status });
            
            entity.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerId);
            entity.HasOne(e => e.Dealer).WithMany().HasForeignKey(e => e.DealerId);
            entity.HasOne(e => e.DealerStaff).WithMany().HasForeignKey(e => e.DealerStaffId);
            entity.HasOne(e => e.VehicleVariant).WithMany().HasForeignKey(e => e.VehicleVariantId);
            entity.HasOne(e => e.VehicleColor).WithMany().HasForeignKey(e => e.VehicleColorId);
        });

        // Order
        builder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            entity.HasIndex(e => new { e.DealerId, e.OrderDate });
            entity.HasIndex(e => e.Status);
            
            entity.HasOne(e => e.Quotation).WithOne(q => q.Order).HasForeignKey<Order>(e => e.QuotationId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Customer).WithMany(c => c.Orders).HasForeignKey(e => e.CustomerId);
            entity.HasOne(e => e.Dealer).WithMany(d => d.Orders).HasForeignKey(e => e.DealerId);
            entity.HasOne(e => e.DealerStaff).WithMany(ds => ds.Orders).HasForeignKey(e => e.DealerStaffId);
            entity.HasOne(e => e.VehicleVariant).WithMany().HasForeignKey(e => e.VehicleVariantId);
            entity.HasOne(e => e.VehicleColor).WithMany().HasForeignKey(e => e.VehicleColorId);
            entity.HasOne(e => e.VehicleInventory).WithMany().HasForeignKey(e => e.VehicleInventoryId).OnDelete(DeleteBehavior.SetNull);
        });

        // Sales Contract
        builder.Entity<SalesContract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ContractNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.ContractNumber).IsUnique();
            entity.HasOne(e => e.Order).WithOne(o => o.SalesContract).HasForeignKey<SalesContract>(e => e.OrderId);
        });

        // Delivery
        builder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeliveryCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.DeliveryCode).IsUnique();
            entity.HasOne(e => e.Order).WithOne(o => o.Delivery).HasForeignKey<Delivery>(e => e.OrderId);
        });

        // Payment
        builder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PaymentCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.PaymentCode).IsUnique();
            entity.HasIndex(e => new { e.OrderId, e.Status });
            entity.HasOne(e => e.Order).WithMany(o => o.Payments).HasForeignKey(e => e.OrderId);
        });

        // Installment Plan
        builder.Entity<InstallmentPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PlanCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.PlanCode).IsUnique();
            entity.HasOne(e => e.Order).WithMany().HasForeignKey(e => e.OrderId);
        });

        // Test Drive
        builder.Entity<TestDrive>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.DealerId, e.ScheduledDate });
            entity.HasOne(e => e.Customer).WithMany(c => c.TestDrives).HasForeignKey(e => e.CustomerId);
            entity.HasOne(e => e.Dealer).WithMany().HasForeignKey(e => e.DealerId);
            entity.HasOne(e => e.VehicleVariant).WithMany().HasForeignKey(e => e.VehicleVariantId);
            entity.HasOne(e => e.AssignedStaff).WithMany().HasForeignKey(e => e.AssignedStaffId).OnDelete(DeleteBehavior.SetNull);
        });

        // Customer Feedback
        builder.Entity<CustomerFeedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.DealerId, e.FeedbackStatus });
            entity.HasOne(e => e.Customer).WithMany(c => c.Feedbacks).HasForeignKey(e => e.CustomerId);
            entity.HasOne(e => e.Order).WithMany().HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Dealer).WithMany().HasForeignKey(e => e.DealerId);
            entity.HasOne(e => e.Responder).WithMany().HasForeignKey(e => e.RespondedBy).OnDelete(DeleteBehavior.SetNull);
        });

        // Promotion
        builder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PromotionCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.PromotionCode).IsUnique();
            entity.HasIndex(e => new { e.Status, e.StartDate, e.EndDate });
        });

        // Order Promotion
        builder.Entity<OrderPromotion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order).WithMany(o => o.Promotions).HasForeignKey(e => e.OrderId);
            entity.HasOne(e => e.Promotion).WithMany(p => p.OrderPromotions).HasForeignKey(e => e.PromotionId);
        });

        // Dealer Discount Policy
        builder.Entity<DealerDiscountPolicy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.DealerId, e.IsActive });
            entity.HasOne(e => e.Dealer).WithMany().HasForeignKey(e => e.DealerId);
            entity.HasOne(e => e.VehicleVariant).WithMany().HasForeignKey(e => e.VehicleVariantId).OnDelete(DeleteBehavior.SetNull);
        });

        // Dealer Debt
        builder.Entity<DealerDebt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DebtCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => new { e.DealerId, e.Status });
            entity.HasOne(e => e.Dealer).WithMany(d => d.Debts).HasForeignKey(e => e.DealerId);
        });

        // Customer Debt
        builder.Entity<CustomerDebt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DebtCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => new { e.CustomerId, e.Status });
            entity.HasOne(e => e.Customer).WithMany().HasForeignKey(e => e.CustomerId);
            entity.HasOne(e => e.Order).WithMany().HasForeignKey(e => e.OrderId);
        });

        // Vehicle Request
        builder.Entity<VehicleRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestCode).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.RequestCode).IsUnique();
            entity.HasIndex(e => new { e.DealerId, e.Status });
            
            entity.HasOne(e => e.Dealer).WithMany().HasForeignKey(e => e.DealerId);
            entity.HasOne(e => e.VehicleVariant).WithMany().HasForeignKey(e => e.VehicleVariantId);
            entity.HasOne(e => e.VehicleColor).WithMany().HasForeignKey(e => e.VehicleColorId);
            entity.HasOne(e => e.Approver).WithMany().HasForeignKey(e => e.ApprovedBy).OnDelete(DeleteBehavior.SetNull);
        });

        // Apply global query filters for soft delete
        builder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<VehicleModel>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<VehicleVariant>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<VehicleColor>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<VehicleInventory>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Dealer>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Order>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Quotation>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update audit fields
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.MarkAsCreated(_currentUserService.UserId);
                    break;
                case EntityState.Modified:
                    entry.Entity.MarkAsModified(_currentUserService.UserId);
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

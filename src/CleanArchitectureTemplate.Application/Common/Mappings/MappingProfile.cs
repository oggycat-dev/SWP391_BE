using AutoMapper;
using System.Text.Json;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using CleanArchitectureTemplate.Application.Common.DTOs.Users;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;
using CleanArchitectureTemplate.Application.Common.DTOs.Dealers;
using CleanArchitectureTemplate.Application.Common.DTOs.Customers;
using CleanArchitectureTemplate.Application.Common.DTOs.Quotations;
using CleanArchitectureTemplate.Application.Common.DTOs.Orders;
using CleanArchitectureTemplate.Application.Common.DTOs.Payments;
using CleanArchitectureTemplate.Application.Common.DTOs.VehicleRequests;
using CleanArchitectureTemplate.Application.Common.DTOs.TestDrives;
using CleanArchitectureTemplate.Application.Common.DTOs.Sales;

namespace CleanArchitectureTemplate.Application.Common.Mappings;

/// <summary>
/// Mapping profile for AutoMapper
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ConstructUsing(src => new UserDto
            {
                Id = src.Id,
                Username = src.Email.Contains('@') ? src.Email.Substring(0, src.Email.IndexOf('@')) : src.Email,
                Email = src.Email,
                FirstName = src.FirstName,
                LastName = src.LastName,
                Role = src.Role,
                IsActive = src.IsActive,
                LastLoginDate = null,
                CreatedAt = src.CreatedAt,
                UpdatedAt = src.ModifiedAt
            });
            
        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

        // Vehicle Model mappings
        CreateMap<VehicleModel, VehicleModelDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.ImageUrls) 
                    ? new List<string>() 
                    : JsonSerializer.Deserialize<List<string>>(src.ImageUrls, (JsonSerializerOptions)null!) ?? new List<string>()));

        // Vehicle Variant mappings
        CreateMap<VehicleVariant, VehicleVariantDto>()
            .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.ModelName))
            .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.Specifications) 
                    ? new Dictionary<string, object>() 
                    : JsonSerializer.Deserialize<Dictionary<string, object>>(src.Specifications, (JsonSerializerOptions)null!) ?? new Dictionary<string, object>()));

        // Vehicle Color mappings
        CreateMap<VehicleColor, VehicleColorDto>()
            .ForMember(dest => dest.VariantName, opt => opt.MapFrom(src => src.Variant.VariantName));

        // Vehicle Inventory mappings
        CreateMap<VehicleInventory, VehicleInventoryDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.WarehouseLocation.ToString()))
            .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Variant.Model.ModelName))
            .ForMember(dest => dest.VariantName, opt => opt.MapFrom(src => src.Variant.VariantName))
            .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer != null ? src.Dealer.Name : null));

        // Dealer mappings
        CreateMap<Dealer, DealerDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Name));

        // Dealer Staff mappings
        CreateMap<DealerStaff, DealerStaffDto>()
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        // Dealer Contract mappings
        CreateMap<DealerContract, DealerContractDto>()
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // Dealer Debt mappings
        CreateMap<DealerDebt, DealerDebtDto>()
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.DebtType, opt => opt.MapFrom(src => "Dealer Debt"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.TotalDebt))
            .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => src.TotalDebt - src.PaidAmount));

        // Customer mappings
        CreateMap<Customer, CustomerDto>();

        // Quotation mappings
        CreateMap<Quotation, QuotationDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.VehicleVariantName, opt => opt.MapFrom(src => src.VehicleVariant.VariantName))
            .ForMember(dest => dest.VehicleColorName, opt => opt.MapFrom(src => src.VehicleColor.ColorName));

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.VehicleVariantName, opt => opt.MapFrom(src => src.VehicleVariant.VariantName))
            .ForMember(dest => dest.VehicleModelName, opt => opt.MapFrom(src => src.VehicleVariant.Model.ModelName))
            .ForMember(dest => dest.VehicleColorName, opt => opt.MapFrom(src => src.VehicleColor.ColorName));

        // Payment mappings
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order.OrderNumber))
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.PaymentMethod));

        // Installment Plan mappings
        CreateMap<InstallmentPlan, InstallmentPlanDto>();

        // Vehicle Request mappings
        CreateMap<VehicleRequest, VehicleRequestDto>()
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.VehicleVariantName, opt => opt.MapFrom(src => src.VehicleVariant.VariantName))
            .ForMember(dest => dest.VehicleModelName, opt => opt.MapFrom(src => src.VehicleVariant.Model.ModelName))
            .ForMember(dest => dest.VehicleColorName, opt => opt.MapFrom(src => src.VehicleColor.ColorName));

        // Test Drive mappings
        CreateMap<TestDrive, TestDriveDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.VehicleVariantName, opt => opt.MapFrom(src => src.VehicleVariant.VariantName))
            .ForMember(dest => dest.VehicleModelName, opt => opt.MapFrom(src => src.VehicleVariant.Model.ModelName))
            .ForMember(dest => dest.AssignedStaffName, opt => opt.MapFrom(src => src.AssignedStaff != null ? src.AssignedStaff.User.FirstName + " " + src.AssignedStaff.User.LastName : null));

        // Sales Contract mappings
        CreateMap<SalesContract, SalesContractDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Order.Customer.FullName))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Order.CustomerId))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Order.Dealer.Name))
            .ForMember(dest => dest.DealerId, opt => opt.MapFrom(src => src.Order.DealerId))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order.OrderNumber))
            .ForMember(dest => dest.ContractDate, opt => opt.MapFrom(src => src.SignedDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
            .ForMember(dest => dest.SpecialTerms, opt => opt.MapFrom(src => src.Terms));
    }
}

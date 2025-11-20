using AutoMapper;
using System.Text.Json;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using CleanArchitectureTemplate.Application.Common.DTOs.Users;
using CleanArchitectureTemplate.Application.Common.DTOs.Vehicles;

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
    }
}

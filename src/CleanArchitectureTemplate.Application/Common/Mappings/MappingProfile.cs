using AutoMapper;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Enums;
using CleanArchitectureTemplate.Application.Common.DTOs.Users;

namespace CleanArchitectureTemplate.Application.Common.Mappings;

/// <summary>
/// Mapping profile for AutoMapper
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
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
    }
}

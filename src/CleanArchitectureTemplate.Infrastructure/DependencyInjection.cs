using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Models;
using CleanArchitectureTemplate.Infrastructure.Persistence;
using CleanArchitectureTemplate.Infrastructure.Services;
using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureTemplate.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add database context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Configure storage settings
        services.Configure<StorageSettings>(configuration.GetSection("Storage"));

        // Register interfaces
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IStorageService, LocalStorageService>();
        services.AddScoped<IFileService, FileService>();

        // Add hosted services
        services.AddHostedService<AdminAccountInitializer>();

        return services;
    }
}

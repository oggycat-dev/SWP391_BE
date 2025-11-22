using Microsoft.AspNetCore.Authorization;

namespace CleanArchitectureTemplate.API.Configurations;

/// <summary>
/// Authorization policies configuration for the Electric Vehicle Management System
/// </summary>
public static class AuthorizationConfiguration
{
    /// <summary>
    /// Add authorization policies to the service collection
    /// </summary>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Admin-only policies
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            // EVM (Electric Vehicle Manufacturer) policies
            options.AddPolicy("EVMAccess", policy =>
                policy.RequireRole("Admin", "EVMManager", "EVMStaff"));

            options.AddPolicy("EVMManagement", policy =>
                policy.RequireRole("Admin", "EVMManager"));

            // Dealer policies
            options.AddPolicy("DealerAccess", policy =>
                policy.RequireRole("DealerManager", "DealerStaff"));

            options.AddPolicy("DealerManagement", policy =>
                policy.RequireRole("DealerManager"));

            // Vehicle management policies
            options.AddPolicy("ManageVehicles", policy =>
                policy.RequireRole("Admin", "EVMManager", "EVMStaff"));

            options.AddPolicy("ViewVehicles", policy =>
                policy.RequireRole("Admin", "EVMManager", "EVMStaff", "DealerManager", "DealerStaff"));

            // Customer management policies
            options.AddPolicy("ManageCustomers", policy =>
                policy.RequireRole("DealerManager", "DealerStaff"));

            // Order management policies
            options.AddPolicy("CreateOrders", policy =>
                policy.RequireRole("DealerManager", "DealerStaff"));

            options.AddPolicy("ApproveOrders", policy =>
                policy.RequireRole("Admin", "EVMManager", "DealerManager"));

            // Vehicle request policies
            options.AddPolicy("CreateVehicleRequests", policy =>
                policy.RequireRole("DealerManager", "DealerStaff"));

            options.AddPolicy("ApproveVehicleRequests", policy =>
                policy.RequireRole("Admin", "EVMManager"));

            // Reports policies
            options.AddPolicy("ViewReports", policy =>
                policy.RequireRole("Admin", "EVMManager", "EVMStaff", "DealerManager"));

            options.AddPolicy("ViewAllReports", policy =>
                policy.RequireRole("Admin", "EVMManager"));

            // Dealer management policies
            options.AddPolicy("ManageDealers", policy =>
                policy.RequireRole("Admin", "EVMManager"));

            // User management policies
            options.AddPolicy("ManageUsers", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("ManageDealerStaff", policy =>
                policy.RequireRole("Admin", "EVMManager", "DealerManager"));
        });

        return services;
    }
}


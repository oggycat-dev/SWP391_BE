using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CleanArchitectureTemplate.API.Configurations;

/// <summary>
/// Swagger configuration
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Add Swagger services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // CMS Document (Admin & EVM Management APIs)
            c.SwaggerDoc("cms", new OpenApiInfo
            {
                Title = "Clean Architecture Template - CMS API",
                Version = "v1",
                Description = "Admin & EVM Management APIs for managing vehicles, dealers, and system operations",
                Contact = new OpenApiContact
                {
                    Name = "Developer",
                    Email = "developer@example.com"
                }
            });

            // Dealer Document (Dealer Management APIs)
            c.SwaggerDoc("dealer", new OpenApiInfo
            {
                Title = "Clean Architecture Template - Dealer API",
                Version = "v1",
                Description = "APIs for dealer managers and staff to manage their operations",
                Contact = new OpenApiContact
                {
                    Name = "Developer",
                    Email = "developer@example.com"
                }
            });

            // Customer Document (Customer APIs)
            c.SwaggerDoc("customer", new OpenApiInfo
            {
                Title = "Clean Architecture Template - Customer API",
                Version = "v1",
                Description = "Public APIs for customer operations including authentication, vehicle browsing, and orders",
                Contact = new OpenApiContact
                {
                    Name = "Developer",
                    Email = "developer@example.com"
                }
            });

            // Group APIs by ApiExplorerSettings GroupName attribute
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (apiDesc.ActionDescriptor.EndpointMetadata
                    .OfType<Microsoft.AspNetCore.Mvc.ApiExplorerSettingsAttribute>()
                    .FirstOrDefault()?.GroupName == docName)
                {
                    return true;
                }

                // Fallback: Include old API controllers in cms for backward compatibility
                if (string.IsNullOrEmpty(apiDesc.RelativePath))
                    return false;

                if (docName == "cms")
                {
                    return apiDesc.RelativePath.StartsWith("api/cms/", StringComparison.OrdinalIgnoreCase);
                }

                return false;
            });

            // Include XML comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Add JWT authentication to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Use Swagger middleware
    /// </summary>
    /// <param name="app">Web application</param>
    /// <param name="environment">Environment</param>
    /// <returns>Web application</returns>
    public static WebApplication UseSwaggerConfiguration(this WebApplication app, IWebHostEnvironment environment)
    {
        // Enable Swagger in all environments (including Production for demo)
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // CMS API endpoint (Admin & EVM)
            c.SwaggerEndpoint("/swagger/cms/swagger.json", "CMS API v1");
            
            // Dealer API endpoint
            c.SwaggerEndpoint("/swagger/dealer/swagger.json", "Dealer API v1");
            
            // Customer API endpoint
            c.SwaggerEndpoint("/swagger/customer/swagger.json", "Customer API v1");
            
            c.RoutePrefix = "swagger";
            c.DisplayRequestDuration();
        });

        return app;
    }
}

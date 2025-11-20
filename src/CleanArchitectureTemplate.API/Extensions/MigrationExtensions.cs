using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.Infrastructure.Persistence;
using Serilog;

namespace CleanArchitectureTemplate.API.Extensions;

/// <summary>
/// Migration extensions
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Apply database migrations before building the app
    /// </summary>
    /// <param name="builder">Web application builder</param>
    /// <param name="logger">Logger</param>
    /// <returns>Task</returns>
    public static async Task ApplyMigrationsAsync(this WebApplicationBuilder builder, Serilog.ILogger logger)
    {
        // Build a temporary service provider to get the context
        var serviceProvider = builder.Services.BuildServiceProvider();
        
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        try
        {
            logger.Information("Checking database connection...");
            
            // Wait for database to be ready (with retry logic)
            var maxRetries = 30;
            var retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    await context.Database.CanConnectAsync();
                    logger.Information("Database connection successful.");
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        logger.Error(ex, "Failed to connect to database after {MaxRetries} attempts", maxRetries);
                        throw;
                    }
                    logger.Warning("Database connection attempt {RetryCount}/{MaxRetries} failed. Retrying in 2 seconds...", retryCount, maxRetries);
                    await Task.Delay(2000);
                }
            }
            
            logger.Information("Applying database migrations...");
            await context.Database.MigrateAsync();
            logger.Information("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "An error occurred while applying database migrations");
            throw;
        }
    }
}

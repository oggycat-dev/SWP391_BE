using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Infrastructure.Persistence;
using CleanArchitectureTemplate.API.Configurations;
using CleanArchitectureTemplate.API.Filters;
using CleanArchitectureTemplate.API.Injection;
using CleanArchitectureTemplate.API.Extensions;
using Serilog;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel for Azure App Service
builder.WebHost.ConfigureKestrel(options =>
{
    // Azure App Service uses PORT environment variable
    var port = Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrEmpty(port))
    {
        options.ListenAnyIP(int.Parse(port));
    }
});

// Add logging configuration
builder.AddLoggingConfiguration();

// Add environment configuration from .env
builder.AddEnvironmentConfiguration();

// Get environment information
var isProduction = builder.Environment.IsProduction();
var isDevelopment = builder.Environment.IsDevelopment();

// Get logger to log startup information
var startupLogger = LoggingConfiguration.CreateStartupLogger();

// Log environment information
startupLogger.Information($"Current Environment: {builder.Environment.EnvironmentName}");
startupLogger.Information($"IsProduction: {isProduction}");
startupLogger.Information($"IsDevelopment: {isDevelopment}");
startupLogger.Information($"ConnectionString configured: {!string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection"))}");

// Configure JSON serialization
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilterAttribute>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Add configurations
builder.Services.AddSwaggerConfiguration();
builder.Services.AddCorsConfiguration(builder.Configuration);

// Add application and infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add API services
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();

// Add HTTP context accessor for current user service
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Apply migrations AFTER building the app but BEFORE starting it
using (var scope = app.Services.CreateScope())
{
    try
    {
        startupLogger.Information("Initializing database...");
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        startupLogger.Information("Checking database connection...");
        var maxRetries = 30;
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                await context.Database.CanConnectAsync();
                startupLogger.Information("Database connection successful.");
                break;
            }
            catch (Exception ex)
            {
                if (i >= maxRetries - 1)
                {
                    startupLogger.Error(ex, "Failed to connect to database after {MaxRetries} attempts", maxRetries);
                    throw;
                }
                startupLogger.Warning("Database connection attempt {RetryCount}/{MaxRetries} failed. Retrying in 2 seconds...", i + 1, maxRetries);
                await Task.Delay(2000);
            }
        }
        
        // Migrations are now handled by docker-entrypoint.sh script
        // This ensures migrations run before the app starts
        startupLogger.Information("Database connection verified. Migrations handled by entrypoint script.");
        startupLogger.Information("Database initialization completed successfully.");
    }
    catch (Exception ex)
    {
        startupLogger.Error(ex, "An error occurred during database initialization!");
        throw;
    }
}

// Configure the HTTP request pipeline

// Add global exception handling middleware
app.UseGlobalExceptionHandling();

// Apply Swagger configuration
app.UseSwaggerConfiguration(app.Environment);

// Ensure wwwroot exists
var wwwrootPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
}

// Add static files middleware - serve from wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wwwrootPath),
    RequestPath = ""
});

if (isProduction)
{
    app.UseHttpsRedirection();
}

// Handle CORS
app.UseCorsConfiguration();

// Add routing
app.UseRouting();

// Add authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

try
{
    startupLogger.Information("Starting web host");
    await app.RunAsync();
}
catch (Exception ex)
{
    startupLogger.Error(ex, "Host terminated unexpectedly");
    throw;
}
finally
{
    // Ensure all logs are written
    Log.CloseAndFlush();
}

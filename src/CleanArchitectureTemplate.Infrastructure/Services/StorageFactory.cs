using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Application.Common.Models;

namespace CleanArchitectureTemplate.Infrastructure.Services;

/// <summary>
/// Factory for creating storage service instances based on configuration
/// </summary>
public class StorageFactory : IStorageFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly StorageSettings _storageSettings;

    public StorageFactory(IServiceProvider serviceProvider, IOptions<StorageSettings> storageSettings)
    {
        _serviceProvider = serviceProvider;
        _storageSettings = storageSettings.Value;
    }

    public IStorageService CreateStorageService()
    {
        return _storageSettings.ProviderType.ToLowerInvariant() switch
        {
            "localstorage" => _serviceProvider.GetRequiredService<LocalStorageService>(),
            // "amazons3" => _serviceProvider.GetRequiredService<AmazonS3StorageService>(),
            // "azureblob" => throw new NotImplementedException("Azure Blob storage is not yet implemented"),
            _ => throw new NotSupportedException($"Storage provider '{_storageSettings.ProviderType}' is not supported")
        };
    }
} 
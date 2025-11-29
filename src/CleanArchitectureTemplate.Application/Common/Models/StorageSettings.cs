namespace CleanArchitectureTemplate.Application.Common.Models;

/// <summary>
/// Storage settings configuration
/// </summary>
public class StorageSettings
{
    public string ProviderType { get; set; } = "LocalStorage";
    public string BaseUrl { get; set; } = string.Empty;
    public LocalStorageSettings LocalStorage { get; set; } = new();
}

/// <summary>
/// Local storage specific settings
/// </summary>
public class LocalStorageSettings
{
    public string RootPath { get; set; } = "wwwroot/uploads";
    public long MaxFileSize { get; set; } = 500 * 1024 * 1024; // 500MB
    public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".epub", ".mp4", ".avi", ".mov", ".mkv", ".zip", ".rar" };
}

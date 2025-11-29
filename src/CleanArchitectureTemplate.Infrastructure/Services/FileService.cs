using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CleanArchitectureTemplate.Application.Common.Interfaces;
using CleanArchitectureTemplate.Infrastructure.Utils;

namespace CleanArchitectureTemplate.Infrastructure.Services;

/// <summary>
/// Implementation of the file service using the appropriate storage provider
/// </summary>
public class FileService : IFileService
{
    private readonly IStorageService _storageService;
    private readonly ILogger<FileService> _logger;

    public FileService(
        IStorageService storageService,
        ILogger<FileService> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a file to storage
    /// </summary>
    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string subDirectory = "")
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty");
        }

        // Create unique, sanitized file name to prevent overwriting and ensure URL safety
        var fileName = FileNameSanitizer.CreateUniqueFileName(file.FileName);
        
        // Use storage service to upload the file
        using var fileStream = file.OpenReadStream();
        var storagePath = await _storageService.UploadFileAsync(fileStream, fileName, file.ContentType, subDirectory);
        
        // Get file extension
        var extension = Path.GetExtension(file.FileName).TrimStart('.');
        
        // Create result object
        var result = new FileUploadResult
        {
            OriginalFileName = file.FileName,
            FileName = fileName,
            SizeKb = file.Length / 1024, // Convert to KB
            FilePath = storagePath,
            MimeType = file.ContentType,
            Extension = extension
        };
        
        _logger.LogInformation("File uploaded successfully: {FileName}", fileName);
        return result;
    }

    /// <summary>
    /// Delete a file from storage
    /// </summary>
    public async Task<bool> DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path is empty");
        }
        
        // Use storage service to delete the file
        var success = await _storageService.DeleteFileAsync(filePath);
        
        if (success)
        {
            _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
        }
        else
        {
            _logger.LogWarning("File could not be deleted: {FilePath}", filePath);
        }
        
        return success;
    }

    /// <summary>
    /// Get file from storage by path
    /// </summary>
    public async Task<(byte[] FileContent, string ContentType, string FileName)> GetFileAsync(string filePath, string originalFileName)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path is empty");
        }

        // Get the file content from storage
        var fileStream = await _storageService.DownloadFileAsync(filePath);
        
        if (fileStream == null)
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        var fileContent = memoryStream.ToArray();

        if (fileContent.Length == 0)
        {
            throw new InvalidOperationException("File is empty");
        }

        // Get content type from file info
        var fileInfo = await _storageService.GetFileInfoAsync(filePath);
        var contentType = fileInfo?.ContentType ?? "application/octet-stream";

        // Use the original file name if available, otherwise use the file name from the path
        string fileName = !string.IsNullOrEmpty(originalFileName) 
            ? originalFileName 
            : Path.GetFileName(filePath);

        return (fileContent, contentType, fileName);
    }

    /// <summary>
    /// Get the public URL for a file
    /// </summary>
    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            _logger.LogWarning("File path is empty when getting file URL");
            return string.Empty;
        }

        return _storageService.GetFileUrl(filePath);
    }
} 
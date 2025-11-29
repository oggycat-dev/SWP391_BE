using Microsoft.AspNetCore.Http;

namespace CleanArchitectureTemplate.Application.Common.Interfaces;

/// <summary>
/// Interface for file operations service
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Upload a file to storage
    /// </summary>
    Task<FileUploadResult> UploadFileAsync(IFormFile file, string subDirectory = "");

    /// <summary>
    /// Delete a file from storage
    /// </summary>
    Task<bool> DeleteFileAsync(string filePath);

    /// <summary>
    /// Get file from storage by path
    /// </summary>
    Task<(byte[] FileContent, string ContentType, string FileName)> GetFileAsync(string filePath, string originalFileName);

    /// <summary>
    /// Get the public URL for a file
    /// </summary>
    string GetFileUrl(string filePath);
}

/// <summary>
/// File upload result model
/// </summary>
public class FileUploadResult
{
    public string OriginalFileName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long SizeKb { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
} 
using AutoMapper;
using CleanArchitectureTemplate.Application.Common.Interfaces;

namespace CleanArchitectureTemplate.Application.Common.Mappings;

/// <summary>
/// AutoMapper value resolver for converting relative image paths to full URLs
/// </summary>
public class ImageUrlListResolver : IValueResolver<object, object, List<string>>
{
    private readonly IFileService _fileService;

    public ImageUrlListResolver(IFileService fileService)
    {
        _fileService = fileService;
    }

    public List<string> Resolve(object source, object destination, List<string> destMember, ResolutionContext context)
    {
        var imageUrls = context.Items.TryGetValue("ImageUrls", out var urls) 
            ? urls as List<string> 
            : new List<string>();

        if (imageUrls == null || !imageUrls.Any())
            return new List<string>();

        return imageUrls.Select(url => _fileService.GetFileUrl(url)).ToList();
    }
}

/// <summary>
/// AutoMapper value resolver for converting single relative image path to full URL
/// </summary>
public class ImageUrlResolver : IValueResolver<object, object, string>
{
    private readonly IFileService _fileService;

    public ImageUrlResolver(IFileService fileService)
    {
        _fileService = fileService;
    }

    public string Resolve(object source, object destination, string destMember, ResolutionContext context)
    {
        var imageUrl = context.Items.TryGetValue("ImageUrl", out var url) 
            ? url as string 
            : string.Empty;

        if (string.IsNullOrEmpty(imageUrl))
            return string.Empty;

        return _fileService.GetFileUrl(imageUrl);
    }
}

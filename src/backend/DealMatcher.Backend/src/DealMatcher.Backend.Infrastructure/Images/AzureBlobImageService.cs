using MailKit.Net.Imap;
using Microsoft.AspNetCore.Http;

namespace DealMatcher.Backend.Infrastructure.Images;

public class AzureBlobImageService : IImageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<AzureBlobImageService> _logger;

    private readonly long _maxFileSize = 5 * 1024 * 1024;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private readonly string[] _allowedMimeTypes = ["image/jpeg", "image/png", "image/gif", "image/webp"];

    public AzureBlobImageService(IConfiguration config, ILogger<AzureBlobImageService> logger)
    {
        _logger = logger;
        var connectionString = config.GetValue<string>("AzureBlob:ConnectionString");
        var containerName = config.GetValue<string>("AzureBlob:ContainerName");
        Guard.Against.NullOrEmpty(connectionString);
        Guard.Against.NullOrEmpty(containerName);
        _containerClient = new BlobContainerClient(connectionString, containerName);
    }

    public async Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
    {
        if (!ValidateImage(image))
        {
            return string.Empty;
        }
        try
        {
            var sanitizedFileName = SanitizeFileName(image.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
            var blobPath = uniqueFileName;
            var blobClient = _containerClient.GetBlobClient(blobPath);
            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = image.ContentType
            };
            using var stream = image.OpenReadStream();
            await blobClient.UploadAsync(stream, blobHttpHeaders, cancellationToken: cancellationToken);

            var blobUrl = blobClient.Uri.ToString();
            _logger.LogInformation("Image uploaded successfully: {blobUrl} (Size: {size} bytes)", blobUrl, image.Length);

            return blobUrl;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error uploading image {FileName}: {Message}", image.FileName, e.Message);
            throw;
        }
    }

    public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> images, CancellationToken cancellationToken = default)
    {
        var res = new List<string>();
        foreach (var image in images)
        {
            var url = await UploadImageAsync(image, cancellationToken);
            res.Add(url);
        }
        return res;
    }

    public bool ValidateImage(IFormFile img)
    {
        if (img.Length == 0)
        {
            return false;
        }
        if (img.Length > _maxFileSize)
        {
            return false;
        }
        var fileExtension = Path.GetExtension(img.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(fileExtension))
        {
            return false;
        }

        if (!_allowedMimeTypes.Contains(img.ContentType))
        {
            return false;
        }
        return true;
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

        var extension = Path.GetExtension(sanitized);
        var nameWithoutExt = Path.GetFileNameWithoutExtension(sanitized);

        if (nameWithoutExt.Length > 100)
            nameWithoutExt = nameWithoutExt[..100];

        return $"{nameWithoutExt}{extension}";
    }
}

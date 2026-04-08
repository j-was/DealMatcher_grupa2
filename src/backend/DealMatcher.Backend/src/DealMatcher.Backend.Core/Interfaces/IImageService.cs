namespace DealMatcher.Backend.Core.Interfaces;

public interface IImageService
{
    public Task<string> UploadImageAsync(
        IFormFile image,
        CancellationToken cancellationToken = default);

    public Task<List<string>> UploadMultipleImagesAsync(
        List<IFormFile> images,
        CancellationToken cancellationToken = default);
}

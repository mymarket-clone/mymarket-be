using Microsoft.AspNetCore.Http;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IImageService
{
    Task<List<ImageEntity>> UploadAsync(List<IFormFile> Images, CancellationToken cancellationToken);
    Task<ImageEntity> UploadAsync(IFormFile Image, CancellationToken cancellationToken);
    Task DeleteAsync(List<ImageEntity> images, CancellationToken cancellationToken);
    Task DeleteAsync(ImageEntity image, CancellationToken cancellationToken);
}

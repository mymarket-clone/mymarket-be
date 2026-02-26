using Microsoft.AspNetCore.Http;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IImageService
{
    Task<List<ImageEntity>> UploadAsync(List<IFormFile> Images, CancellationToken cancellationToken);
    Task DeleteAsync(IEnumerable<ImageEntity> images, CancellationToken cancellationToken);
}

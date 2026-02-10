using Microsoft.AspNetCore.Http;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IImageService
{
    Task<List<ImageEntity>> Upload(List<IFormFile> Images, CancellationToken cancellationToken);
}

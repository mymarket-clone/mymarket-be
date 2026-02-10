using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Supabase;

namespace Mymarket.Infrastructure.Services;

public class ImageService(Client _client) : IImageService
{
    public async Task<List<ImageEntity>> Upload(List<IFormFile> Images, CancellationToken cancellationToken)
    {
        var uploadedImages = new List<ImageEntity>();

        try
        {
            foreach (var image in Images)
            {
                var uniqueId = Guid.NewGuid();
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream, cancellationToken);

                var fileName = $"{uniqueId}{Path.GetExtension(image.FileName)}";
                await _client.Storage.From("Images").Upload(
                    memoryStream.ToArray(),
                    fileName
                );

                var url = _client.Storage.From("Images").GetPublicUrl(fileName);
                uploadedImages.Add(new ImageEntity
                {
                    Url = url,        
                    UniqueId = uniqueId
                });
            }

            return uploadedImages;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error uploading images", ex);
        }
    }
}

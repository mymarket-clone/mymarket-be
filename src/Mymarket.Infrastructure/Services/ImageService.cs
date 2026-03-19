using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Supabase;
using static System.Net.Mime.MediaTypeNames;

namespace Mymarket.Infrastructure.Services;

public class ImageService(Client _client) : IImageService
{
    public async Task<List<ImageEntity>> UploadAsync(List<IFormFile> Images, CancellationToken cancellationToken)
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
    public async Task<ImageEntity> UploadAsync(IFormFile Image, CancellationToken cancellationToken)
    {
        if (Image is null) throw new ArgumentNullException(nameof(Image));
        if (Image.Length <= 0) throw new ArgumentException("Image is empty.", nameof(Image));

        try
        {
            var uniqueId = Guid.NewGuid();

            using var memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream, cancellationToken);

            var fileName = $"{uniqueId}{Path.GetExtension(Image.FileName)}";

            await _client.Storage
                .From("Images")
                .Upload(memoryStream.ToArray(), fileName);

            var url = _client.Storage.From("Images").GetPublicUrl(fileName);

            return new ImageEntity
            {
                Url = url,
                UniqueId = uniqueId
            };
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error uploading image", ex);
        }
    }

    public async Task DeleteAsync(List<ImageEntity> images, CancellationToken cancellationToken)
    {
        if (images == null) return;

        var fileNames = images
            .Select(i =>
            {
                return Path.GetFileName(i.Url);
            })
            .ToList();

        if (fileNames.Count == 0) return;

        await _client.Storage
            .From("Images")
            .Remove(fileNames);
    }

    public async Task DeleteAsync(ImageEntity image, CancellationToken cancellationToken)
    {
        if (image == null) return;

        var fileName = Path.GetFileName(image.Url);

        if (string.IsNullOrWhiteSpace(fileName)) return;

        await _client.Storage
            .From("Images")
            .Remove([fileName]);
    }
}

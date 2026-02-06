using Microsoft.AspNetCore.Http;
using Supabase;

namespace Mymarket.Domain.Services;

public class ImageService(Client _client)
{
    public async Task<List<string>> Upload(List<IFormFile> Images, CancellationToken cancellationToken)
    {
        var uploadedImageUrls = new List<string>();

        try
        {
            foreach (var image in Images)
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream, cancellationToken);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                await _client.Storage.From("Images").Upload(
                    memoryStream.ToArray(),
                    fileName
                );

                var url = _client.Storage.From("Images").GetPublicUrl(fileName);
                uploadedImageUrls.Add(url);
            }

            return uploadedImageUrls;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error uploading images", ex);
        }
    }
}

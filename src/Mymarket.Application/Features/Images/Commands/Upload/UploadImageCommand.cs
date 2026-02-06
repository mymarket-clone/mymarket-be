using MediatR;
using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Supabase;

namespace Mymarket.Application.Features.Images.Commands.Upload;

public sealed record UploadImageCommand(
    List<IFormFile> Images,
    ImageTargetType TargetType
) : IRequest<List<string>>;

public sealed class UploadImageCommandHandler(IApplicationDbContext _context, Client _client) : IRequestHandler<UploadImageCommand, List<string>>
{
    public async Task<List<string>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var uploadedImageUrls = new List<string>();

        try
        {
            foreach (var image in request.Images)
            {
                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream, cancellationToken);

                var fileName = $"{request.TargetType}/{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
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
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Posts.Commands.Delete;

public record DeletePostCommand(int PostId) : IRequest<Unit>;

public class DeletePostCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IImageService imageService) : IRequestHandler<DeletePostCommand, Unit>
{
    public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(x => x.PostsImages)
                .ThenInclude(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);

        if (post == null || post.UserId != currentUser.Id)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        var images = post.PostsImages
            .Select(x => x.Image)
            .Where(x => x is not null)
            .Select(x => x!)
            .ToList();

        var postViews = context.PostViews.Where(x => x.PostId == request.PostId);
        context.PostViews.RemoveRange(postViews);

        context.Posts.Remove(post);
        await context.SaveChangesAsync(cancellationToken);

        await imageService.DeleteAsync(images, cancellationToken);

        return Unit.Value;
    }
}

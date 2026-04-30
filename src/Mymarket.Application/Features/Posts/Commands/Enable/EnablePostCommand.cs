using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Commands.Enable;

public record EnablePostCommand(int PostId) : IRequest<Unit>;

public class EnablePostCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<EnablePostCommand, Unit>
{
    public async Task<Unit> Handle(EnablePostCommand request, CancellationToken cancellationToken)
    {
        var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);
        if (post == null || post.UserId != currentUser.Id)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        post.Status = PostStatus.Active;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
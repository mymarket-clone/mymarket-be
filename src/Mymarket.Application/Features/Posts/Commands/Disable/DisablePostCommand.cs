using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Commands.Disable;

public record DisablePostCommand(int PostId) : IRequest<Unit>;

public class DisablePostCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<DisablePostCommand, Unit>
{
    public async Task<Unit> Handle(DisablePostCommand request, CancellationToken cancellationToken)
    {
        var post = await context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);
        if (post == null || post.UserId != currentUser.Id)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        post.Status = PostStatus.Inactive;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Views.Commands.View;

public record ViewPostCommand(int PostId) : IRequest<Unit>;

public class ViewPostCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IMemoryCache cache) : IRequestHandler<ViewPostCommand, Unit>
{
    public async Task<Unit> Handle(ViewPostCommand request, CancellationToken cancellationToken)
    {
        var identity = currentUser.Id?.ToString() ?? currentUser.SessionId?.ToString();
        var key = $"post-view:{request.PostId}:{identity}";

        if (cache.TryGetValue(key, out _)) return Unit.Value;


        return Unit.Value;
    }
}
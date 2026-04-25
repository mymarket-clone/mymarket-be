using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Views.Commands.View;

public record ViewPostCommand(int PostId) : IRequest<Unit>;

public class ViewPostCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<ViewPostCommand, Unit>
{
    public async Task<Unit> Handle(ViewPostCommand request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        await context.GetDatabase().ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "PostViews" ("PostId", "UserId", "SessionId", "ViewDate", "ViewedAt")
            VALUES (
                {request.PostId}, 
                {currentUser.Id}, 
                {(currentUser.Id.HasValue ? null : currentUser.SessionId)}, 
                {today}, 
                {DateTime.UtcNow}
            )
            ON CONFLICT DO NOTHING
        """, cancellationToken);

        return Unit.Value;
    }
}
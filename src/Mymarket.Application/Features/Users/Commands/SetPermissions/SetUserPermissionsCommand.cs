using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Users.Commands.SetPermissions;

public record SetUserPermissionsCommand(
    int UserId,
    List<int> Permissions
) : IRequest;

public class SetUserPermissionsCommandHandler(IApplicationDbContext context) : IRequestHandler<SetUserPermissionsCommand>
{
    public async Task Handle(SetUserPermissionsCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        var permissionIds = (request.Permissions ?? []).Distinct().ToList();

        user.Permissions = await context.Permissions
            .Where(x => permissionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}

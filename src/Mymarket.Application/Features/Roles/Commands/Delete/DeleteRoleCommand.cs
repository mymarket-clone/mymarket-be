using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Roles.Commands.Delete;

public record DeleteRoleCommand(int Id) : IRequest;

public class DeleteRoleCommandHandler(
    IApplicationDbContext context) : IRequestHandler<DeleteRoleCommand>
{
    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        context.Roles.Remove(role);
        await context.SaveChangesAsync(cancellationToken);
    }
}

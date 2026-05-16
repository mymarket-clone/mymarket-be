using MediatR;

namespace Mymarket.Application.Features.Permissions.Commands.LinkPermission;

public record LinkPermissionCommand(
    int RoleId,
    Domain.Enums.Permissions PermissionId
) : IRequest;

public class LinkPermissionCommandHandler : IRequestHandler<LinkPermissionCommand>
{
    public Task Handle(LinkPermissionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
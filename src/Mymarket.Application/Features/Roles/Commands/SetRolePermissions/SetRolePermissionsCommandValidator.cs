using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Roles.Commands.SetRolePermissions;

public class SetRolePermissionsCommandValidator : AbstractValidator<SetRolePermissionsCommand>
{
    public SetRolePermissionsCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Permissions)
            .NotEmpty();

        RuleFor(x => x.Permissions)
            .MustAsync(async (permissions, cancellationToken) =>
            {
                var permissionsCount = await dbContext.Permissions
                    .CountAsync(x => permissions.Contains(x.Id), cancellationToken);
                return permissionsCount == permissions.Count;
            })
            .WithMessage("Some permissions do not exist");
    }
}
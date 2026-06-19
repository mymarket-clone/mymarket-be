using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Users.Commands.SetPermissions;

public class SetUserPermissionsCommandValidator : AbstractValidator<SetUserPermissionsCommand>
{
    public SetUserPermissionsCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(x => x.Permissions)
            .NotNull();

        RuleFor(x => x.Permissions)
            .MustAsync(async (permissions, cancellationToken) =>
            {
                if (permissions is null) return false;

                var permissionIds = permissions.Distinct().ToList();
                var permissionsCount = await dbContext.Permissions
                    .CountAsync(x => permissionIds.Contains(x.Id), cancellationToken);

                return permissionsCount == permissionIds.Count;
            })
            .WithMessage("Some permissions do not exist");
    }
}

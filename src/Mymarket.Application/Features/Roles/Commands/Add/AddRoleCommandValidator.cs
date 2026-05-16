using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Roles.Commands.Add;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public AddRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SharedResources.FieldRequired)
            .MaximumLength(100).WithMessage(SharedResources.NameMaxLength)
            .MustAsync(RoleNameDoesNotExist).WithMessage(SharedResources.RecordAlredyExists);

        RuleFor(x => x.PermissionIds)
            .MustAsync(AllPermissionsExist).WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> RoleNameDoesNotExist(string name, CancellationToken cancellationToken)
    {
        return !await _context.Roles.AnyAsync(x => x.Name == name, cancellationToken);
    }

    private async Task<bool> AllPermissionsExist(List<int>? permissionIds, CancellationToken cancellationToken)
    {
        var ids = permissionIds?.Distinct().ToList() ?? [];

        if (ids.Count == 0)
        {
            return true;
        }

        var existingCount = await _context.Permissions
            .CountAsync(x => ids.Contains(x.Id), cancellationToken);

        return existingCount == ids.Count;
    }
}

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Roles.Commands.Edit;

public class EditRoleCommandValidator : AbstractValidator<EditRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public EditRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(SharedResources.FieldRequired)
            .MustAsync(RoleExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SharedResources.FieldRequired)
            .MaximumLength(100).WithMessage(SharedResources.NameMaxLength)
            .MustAsync(RoleNameDoesNotExist).WithMessage(SharedResources.RecordAlredyExists);

        RuleFor(x => x.PermissionIds)
            .MustAsync(AllPermissionsExist).WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> RoleExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Roles.AnyAsync(x => x.Id == id, cancellationToken);
    }

    private async Task<bool> RoleNameDoesNotExist(EditRoleCommand command, string name, CancellationToken cancellationToken)
    {
        return !await _context.Roles
            .AnyAsync(x => x.Name == name && x.Id != command.Id, cancellationToken);
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

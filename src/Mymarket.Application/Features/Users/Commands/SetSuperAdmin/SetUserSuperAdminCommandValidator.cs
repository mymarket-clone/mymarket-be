using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.SetSuperAdmin;

public class SetUserSuperAdminCommandValidator : AbstractValidator<SetUserSuperAdminCommand>
{
    private readonly IApplicationDbContext _context;

    public SetUserSuperAdminCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(UserExists).WithMessage("User does not exist.");

        RuleFor(x => x)
            .MustAsync(CanUnsetSuperAdmin).WithMessage("Cannot unset the last SuperAdmin.");
    }

    private async Task<bool> UserExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    private async Task<bool> CanUnsetSuperAdmin(SetUserSuperAdminCommand command, CancellationToken cancellationToken)
    {
        if (command.IsSuperAdmin)
        {
            return true;
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (user is null || user.AccessLevel != AccessLevelType.SuperAdmin)
        {
            return true;
        }

        var superAdminCount = await _context.Users
            .CountAsync(x => x.AccessLevel == AccessLevelType.SuperAdmin, cancellationToken);

        return superAdminCount > 1;
    }
}

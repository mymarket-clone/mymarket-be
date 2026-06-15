using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Helpers;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.Add;

public record AddAdminUserCommand(
    string Firstname,
    string Lastname,
    string Email,
    GenderType Gender,
    int BirthYear,
    string PhoneNumber,
    string Password,
    bool EmailVerified,
    bool IsBlocked,
    bool IsSuperAdmin
) : IRequest<int>;

public class AddAdminUserCommandHandler(
    IApplicationDbContext context,
    IEmailNormalizer emailNormalizer) : IRequestHandler<AddAdminUserCommand, int>
{
    public async Task<int> Handle(AddAdminUserCommand request, CancellationToken cancellationToken)
    {
        await EnsureUniqueUserAsync(request.Email, request.PhoneNumber, null, cancellationToken);

        var user = new UserEntity
        {
            Firstname = request.Firstname,
            LastName = request.Lastname,
            Email = request.Email,
            Gender = request.Gender,
            BirthYear = request.BirthYear,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = CryptoHelper.HashPassword(request.Password),
            EmailVerified = request.EmailVerified,
            IsBlocked = request.IsBlocked,
            AccessLevel = request.IsSuperAdmin ? AccessLevelType.SuperAdmin : AccessLevelType.User,
            RefreshTokenExpiry = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    private async Task EnsureUniqueUserAsync(string email, string phoneNumber, int? userId, CancellationToken cancellationToken)
    {
        var normalizedEmail = emailNormalizer.Normalize(email);

        var existing = await context.Users
            .Where(x => x.Email.ToUpper() == normalizedEmail || x.PhoneNumber == phoneNumber)
            .Where(x => userId == null || x.Id != userId)
            .Select(x => new { x.Email, x.PhoneNumber })
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is null)
            return;

        var propertyName = emailNormalizer.Normalize(existing.Email) == normalizedEmail
            ? nameof(AddAdminUserCommand.Email)
            : nameof(AddAdminUserCommand.PhoneNumber);
        var message = propertyName == nameof(AddAdminUserCommand.Email)
            ? SharedResources.EmailAlreadyExists
            : SharedResources.PhoneNumberAlreadyExists;

        throw new ValidationException([new ValidationFailure(propertyName, message)]);
    }
}

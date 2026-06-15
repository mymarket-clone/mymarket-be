using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.GoogleLogin;

public record GoogleLoginCommand(
    string Code,
    string RedirectUri,
    string? Nonce,
    GoogleLoginApplication Application
) : IRequest<AuthDto>;

public enum GoogleLoginApplication
{
    Client = 1,
    Panel = 2
}

public sealed class GoogleLoginCommandHandler(
    IApplicationDbContext context,
    IGoogleAuthService googleAuthService,
    IEmailNormalizer emailNormalizer,
    IAuthSessionService authSessionService) : IRequestHandler<GoogleLoginCommand, AuthDto>
{
    private const string GoogleProvider = "Google";

    public async Task<AuthDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        var googleUser = await googleAuthService.ValidateAuthorizationCodeAsync(
            request.Code,
            request.RedirectUri,
            request.Nonce,
            cancellationToken);

        if (string.IsNullOrWhiteSpace(googleUser.Email))
            throw Validation(nameof(request.Code), "Google did not provide an email address.");

        if (!googleUser.EmailVerified)
            throw Validation(nameof(request.Code), "Google email must be verified.");

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        var existingLogin = await context.UserExternalLogins
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Provider == GoogleProvider && x.ProviderUserId == googleUser.Subject,
                cancellationToken);

        UserEntity user;

        if (existingLogin is not null)
        {
            user = existingLogin.User;
        }
        else
        {
            var normalizedEmail = emailNormalizer.Normalize(googleUser.Email);

            user = await context.Users
                .Include(x => x.ExternalLogins)
                .FirstOrDefaultAsync(
                    x => x.Email.ToUpper() == normalizedEmail,
                    cancellationToken)
                ?? await CreateOrRejectMissingUserAsync(request.Application, googleUser, cancellationToken);

            var existingGoogleLogin = user.ExternalLogins
                .FirstOrDefault(x => x.Provider == GoogleProvider);

            if (existingGoogleLogin is not null && existingGoogleLogin.ProviderUserId != googleUser.Subject)
                throw Validation(nameof(request.Code), "This user is already linked to another Google account.");

            context.UserExternalLogins.Add(new UserExternalLoginEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Provider = GoogleProvider,
                ProviderUserId = googleUser.Subject,
                ProviderEmail = googleUser.Email
            });
        }

        if (user.IsBlocked)
            throw new UnauthorizedAccessException("Blocked users cannot authenticate.");

        if (request.Application == GoogleLoginApplication.Panel && user.AccessLevel < AccessLevelType.Admin)
            throw new UnauthorizedAccessException("You do not have access to the panel.");

        var auth = await authSessionService.CreateSessionAsync(user, cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return auth;
    }

    private async Task<UserEntity> CreateOrRejectMissingUserAsync(
        GoogleLoginApplication application,
        GoogleUserInfo googleUser,
        CancellationToken cancellationToken)
    {
        if (application == GoogleLoginApplication.Panel)
            throw new UnauthorizedAccessException("No panel account is associated with this Google email.");

        var firstName = string.IsNullOrWhiteSpace(googleUser.GivenName) ? googleUser.Email.Split('@')[0] : googleUser.GivenName;
        var lastName = string.IsNullOrWhiteSpace(googleUser.FamilyName) ? string.Empty : googleUser.FamilyName;

        var user = new UserEntity
        {
            Firstname = firstName,
            LastName = lastName,
            Email = googleUser.Email.Trim(),
            PhoneNumber = null,
            Gender = GenderType.Male,
            BirthYear = DateTime.UtcNow.Year,
            PasswordHash = null,
            EmailVerified = true,
            IsBlocked = false,
            AccessLevel = AccessLevelType.User,
            RefreshTokenExpiry = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return user;
    }

    private static ValidationException Validation(string propertyName, string message)
    {
        return new ValidationException([new ValidationFailure(propertyName, message)]);
    }
}

public sealed class GoogleLoginCommandValidator : AbstractValidator<GoogleLoginCommand>
{
    public GoogleLoginCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.RedirectUri).NotEmpty();
        RuleFor(x => x.Application).IsInEnum();
    }
}

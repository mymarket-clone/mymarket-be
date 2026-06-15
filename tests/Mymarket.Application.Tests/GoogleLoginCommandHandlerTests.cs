using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mymarket.Application.Features.Users.Commands.GoogleLogin;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Services;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Models;
using Mymarket.Infrastructure.Data;

namespace Mymarket.Application.Tests;

public sealed class GoogleLoginCommandHandlerTests
{
    [Fact]
    public async Task Client_google_login_creates_new_regular_user()
    {
        await using var context = CreateContext();
        var handler = CreateHandler(context, GoogleUser("google-1", "New.User@example.com"));

        var result = await handler.Handle(ClientCommand(), CancellationToken.None);

        var user = await context.Users.SingleAsync();
        Assert.Equal(user.Id, result.User.Id);
        Assert.Equal(AccessLevelType.User, user.AccessLevel);
        Assert.True(user.EmailVerified);
        Assert.Null(user.PasswordHash);
        Assert.Single(context.UserExternalLogins);
    }

    [Fact]
    public async Task Client_google_login_links_existing_password_user_with_normalized_email()
    {
        await using var context = CreateContext();
        var user = AddUser(context, email: "existing@example.com", passwordHash: "hash");
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "Existing@Example.com"));

        var result = await handler.Handle(ClientCommand(), CancellationToken.None);

        Assert.Equal(user.Id, result.User.Id);
        Assert.Equal("hash", user.PasswordHash);
        var login = await context.UserExternalLogins.SingleAsync();
        Assert.Equal(user.Id, login.UserId);
    }

    [Fact]
    public async Task Repeated_google_login_uses_existing_external_login_record()
    {
        await using var context = CreateContext();
        var user = AddUser(context, email: "old@example.com");
        context.UserExternalLogins.Add(Login(user, "google-1", "old@example.com"));
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "new@example.com"));

        var result = await handler.Handle(ClientCommand(), CancellationToken.None);

        Assert.Equal(user.Id, result.User.Id);
        Assert.Equal(1, await context.UserExternalLogins.CountAsync());
    }

    [Fact]
    public async Task Panel_google_login_does_not_create_missing_user()
    {
        await using var context = CreateContext();
        var handler = CreateHandler(context, GoogleUser("google-1", "missing@example.com"));

        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => handler.Handle(PanelCommand(), CancellationToken.None));

        Assert.Equal("No panel account is associated with this Google email.", ex.Message);
        Assert.Empty(context.Users);
        Assert.Empty(context.UserExternalLogins);
    }

    [Fact]
    public async Task Panel_google_login_links_existing_permitted_user()
    {
        await using var context = CreateContext();
        var user = AddUser(context, email: "admin@example.com", accessLevel: AccessLevelType.Admin);
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "ADMIN@example.com"));

        var result = await handler.Handle(PanelCommand(), CancellationToken.None);

        Assert.Equal(user.Id, result.User.Id);
        Assert.Single(context.UserExternalLogins);
    }

    [Fact]
    public async Task Panel_google_login_rejects_existing_user_without_panel_access()
    {
        await using var context = CreateContext();
        AddUser(context, email: "client@example.com", accessLevel: AccessLevelType.User);
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "client@example.com"));

        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => handler.Handle(PanelCommand(), CancellationToken.None));

        Assert.Equal("You do not have access to the panel.", ex.Message);
    }

    [Fact]
    public async Task Unverified_google_email_is_rejected()
    {
        await using var context = CreateContext();
        var handler = CreateHandler(context, GoogleUser("google-1", "user@example.com", verified: false));

        await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(ClientCommand(), CancellationToken.None));

        Assert.Empty(context.Users);
    }

    [Fact]
    public async Task Google_identity_cannot_be_linked_to_multiple_users()
    {
        await using var context = CreateContext();
        var linkedUser = AddUser(context, email: "linked@example.com");
        AddUser(context, email: "other@example.com");
        context.UserExternalLogins.Add(Login(linkedUser, "google-1", "linked@example.com"));
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "other@example.com"));

        var result = await handler.Handle(ClientCommand(), CancellationToken.None);

        Assert.Equal(linkedUser.Id, result.User.Id);
        Assert.Equal(1, await context.UserExternalLogins.CountAsync());
    }

    [Fact]
    public async Task User_cannot_have_multiple_google_identities()
    {
        await using var context = CreateContext();
        var user = AddUser(context, email: "user@example.com");
        context.UserExternalLogins.Add(Login(user, "google-existing", "user@example.com"));
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-new", "user@example.com"));

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => handler.Handle(ClientCommand(), CancellationToken.None));

        Assert.Contains("already linked", ex.Errors.Single().ErrorMessage);
    }

    [Fact]
    public async Task Blocked_users_cannot_authenticate_through_google()
    {
        await using var context = CreateContext();
        AddUser(context, email: "blocked@example.com", isBlocked: true);
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "blocked@example.com"));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => handler.Handle(ClientCommand(), CancellationToken.None));
    }

    [Fact]
    public async Task Existing_password_hash_remains_unchanged_after_linking()
    {
        await using var context = CreateContext();
        var user = AddUser(context, email: "password@example.com", passwordHash: "original-hash");
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "password@example.com"));

        await handler.Handle(ClientCommand(), CancellationToken.None);

        Assert.Equal("original-hash", user.PasswordHash);
    }

    [Fact]
    public async Task Google_login_issues_normal_access_and_refresh_tokens()
    {
        await using var context = CreateContext();
        AddUser(context, email: "token@example.com");
        await context.SaveChangesAsync(CancellationToken.None);
        var handler = CreateHandler(context, GoogleUser("google-1", "token@example.com"));

        var result = await handler.Handle(ClientCommand(), CancellationToken.None);
        var user = await context.Users.SingleAsync();

        Assert.Equal($"access-{user.Id}", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.Equal("refresh-token", user.RefreshToken);
        Assert.True(user.RefreshTokenExpiry > DateTime.UtcNow);
    }

    private static GoogleLoginCommandHandler CreateHandler(
        ApplicationDbContext context,
        GoogleUserInfo googleUser)
    {
        return new GoogleLoginCommandHandler(
            context,
            new StubGoogleAuthService(googleUser),
            new EmailNormalizer(),
            new AuthSessionService(context, new StubTokenProvider()));
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new ApplicationDbContext(options);
    }

    private static GoogleLoginCommand ClientCommand() => new(
        Code: "code",
        RedirectUri: "http://localhost:4200/user/google-callback",
        Nonce: "nonce",
        Application: GoogleLoginApplication.Client);

    private static GoogleLoginCommand PanelCommand() => ClientCommand() with
    {
        RedirectUri = "http://localhost:5173/google-callback",
        Application = GoogleLoginApplication.Panel
    };

    private static GoogleUserInfo GoogleUser(string subject, string email, bool verified = true) => new(
        Subject: subject,
        Email: email,
        EmailVerified: verified,
        GivenName: "Google",
        FamilyName: "User");

    private static UserEntity AddUser(
        ApplicationDbContext context,
        string email,
        string? passwordHash = null,
        AccessLevelType accessLevel = AccessLevelType.User,
        bool isBlocked = false)
    {
        var user = new UserEntity
        {
            Firstname = "Test",
            LastName = "User",
            Email = email,
            Gender = GenderType.Male,
            BirthYear = 1990,
            PhoneNumber = $"+995555{context.Users.Local.Count + 100000}",
            PasswordHash = passwordHash,
            EmailVerified = true,
            IsBlocked = isBlocked,
            AccessLevel = accessLevel,
            RefreshTokenExpiry = DateTime.UtcNow
        };

        context.Users.Add(user);
        return user;
    }

    private static UserExternalLoginEntity Login(UserEntity user, string subject, string email) => new()
    {
        Id = Guid.NewGuid(),
        User = user,
        Provider = "Google",
        ProviderUserId = subject,
        ProviderEmail = email
    };

    private sealed class StubGoogleAuthService(GoogleUserInfo user) : IGoogleAuthService
    {
        public Task<GoogleUserInfo> ValidateAuthorizationCodeAsync(
            string code,
            string redirectUri,
            string? nonce,
            CancellationToken cancellationToken) => Task.FromResult(user);
    }

    private sealed class StubTokenProvider : ITokenProvider
    {
        public (string, DateTime) CreateAccessToken(UserModel user)
        {
            return ($"access-{user.Id}", DateTime.UtcNow.AddMinutes(15));
        }

        public (string, DateTime) CreateRefreshToken()
        {
            return ("refresh-token", DateTime.UtcNow.AddDays(30));
        }
    }
}

namespace Mymarket.Application.Interfaces;

public interface IGoogleAuthService
{
    Task<GoogleUserInfo> ValidateAuthorizationCodeAsync(
        string code,
        string redirectUri,
        string? nonce,
        CancellationToken cancellationToken);
}

public sealed record GoogleUserInfo(
    string Subject,
    string Email,
    bool EmailVerified,
    string? GivenName,
    string? FamilyName);

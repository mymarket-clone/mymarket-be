using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Interfaces;

namespace Mymarket.Infrastructure.Authentication;

public sealed class GoogleAuthService(
    GoogleOptions googleOptions,
    IHttpClientFactory httpClientFactory) : IGoogleAuthService
{
    private static readonly ConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager = new(
        "https://accounts.google.com/.well-known/openid-configuration",
        new OpenIdConnectConfigurationRetriever());

    public async Task<GoogleUserInfo> ValidateAuthorizationCodeAsync(
        string code,
        string redirectUri,
        string? nonce,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(googleOptions.ClientId) || string.IsNullOrWhiteSpace(googleOptions.ClientSecret))
            throw new UnauthorizedAccessException("Google authentication is not configured.");

        var tokenResponse = await ExchangeCodeAsync(code, redirectUri, cancellationToken);

        if (string.IsNullOrWhiteSpace(tokenResponse.IdToken))
            throw new UnauthorizedAccessException("Invalid Google authorization code.");

        var configuration = await ConfigurationManager.GetConfigurationAsync(cancellationToken);
        var handler = new JwtSecurityTokenHandler
        {
            MapInboundClaims = false
        };

        try
        {
            var principal = handler.ValidateToken(tokenResponse.IdToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuers = ["https://accounts.google.com", "accounts.google.com"],
                ValidateAudience = true,
                ValidAudience = googleOptions.ClientId,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = configuration.SigningKeys
            }, out _);

            var tokenNonce = principal.FindFirst("nonce")?.Value;
            if (!string.IsNullOrWhiteSpace(nonce) && tokenNonce != nonce)
                throw new UnauthorizedAccessException("Invalid Google token.");

            var subject = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            var emailVerifiedValue = principal.FindFirst("email_verified")?.Value;
            var givenName = principal.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value;
            var familyName = principal.FindFirst(JwtRegisteredClaimNames.FamilyName)?.Value;

            if (string.IsNullOrWhiteSpace(subject))
                throw new UnauthorizedAccessException("Invalid Google token.");

            return new GoogleUserInfo(
                Subject: subject,
                Email: email ?? string.Empty,
                EmailVerified: bool.TryParse(emailVerifiedValue, out var verified) && verified,
                GivenName: givenName,
                FamilyName: familyName);
        }
        catch (SecurityTokenException ex)
        {
            throw new UnauthorizedAccessException("Invalid or expired Google token.", ex);
        }
    }

    private async Task<GoogleTokenResponse> ExchangeCodeAsync(
        string code,
        string redirectUri,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(nameof(GoogleAuthService));

        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = googleOptions.ClientId,
            ["client_secret"] = googleOptions.ClientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        });

        using var response = await client.PostAsync("https://oauth2.googleapis.com/token", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid Google authorization code.");

        return await response.Content.ReadFromJsonAsync<GoogleTokenResponse>(cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid Google authorization code.");
    }

    private sealed class GoogleTokenResponse
    {
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }
    }
}

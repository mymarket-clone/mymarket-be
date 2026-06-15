using System.Text;
using System.Text.Json;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mymarket.Application.features.Users.Commands.LoginUser;
using Mymarket.Application.features.Users.Commands.PasswordRecovery;
using Mymarket.Application.features.Users.Commands.RegisterUser;
using Mymarket.Application.features.Users.Commands.SendEmailVerificationCode;
using Mymarket.Application.features.Users.Commands.SendPasswordRecoveryCode;
using Mymarket.Application.features.Users.Commands.VerifyEmailCodeCommand;
using Mymarket.Application.features.Users.Commands.VerifyPasswodRecoveryCodeCommand;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.features.Users.Queries.UserExists;
using Mymarket.Application.Features.Users.Commands.GoogleLogin;
using Mymarket.Application.Features.Users.Commands.RefreshUser;
using Mymarket.Infrastructure.Authentication;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/auth")]
public class AuthController(
    IMediator mediator,
    GoogleOptions googleOptions) : BaseController
{
    private const string GoogleStateCookie = "mymarket_google_oauth";
    private static readonly JsonSerializerOptions WebJsonOptions = new(JsonSerializerDefaults.Web);

    [HttpPost("register-user")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpPost("send-email-verification-code")]
    public async Task<IActionResult> SendEmailVerificationCode(SendEmailVerificationCodeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("verify-email-code")]
    public async Task<IActionResult> VerifyCode(VerifyEmailCodeCommand command)
    {
        var response = await mediator.Send(command);

        return Ok(response);
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> LoginUser(LoginUserCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("google/client/start")]
    public IActionResult GoogleClientStart([FromQuery] string? returnUrl)
    {
        return StartGoogleLogin(GoogleLoginApplication.Client, returnUrl);
    }

    [HttpGet("google/panel/start")]
    public IActionResult GooglePanelStart([FromQuery] string? returnUrl)
    {
        return StartGoogleLogin(GoogleLoginApplication.Panel, returnUrl);
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error)
    {
        if (!TryReadGoogleState(state, out var googleState))
            return RedirectWithGoogleError(null, "InvalidGoogleState", "Invalid Google login state.");

        Response.Cookies.Delete(GoogleStateCookie);

        if (!string.IsNullOrWhiteSpace(error))
            return RedirectWithGoogleError(googleState.ReturnUrl, "InvalidExternalLogin", error);

        if (string.IsNullOrWhiteSpace(code))
            return RedirectWithGoogleError(googleState.ReturnUrl, "InvalidExternalLogin", "Invalid external login.");

        try
        {
            var response = await mediator.Send(new GoogleLoginCommand(
                code,
                GoogleCallbackUrl(),
                googleState.Nonce,
                googleState.Application));

            return RedirectWithGoogleAuth(googleState.ReturnUrl, response);
        }
        catch (UnauthorizedAccessException exception)
        {
            return RedirectWithGoogleError(googleState.ReturnUrl, "UnauthorizedAccessError", exception.Message);
        }
        catch (ValidationException exception)
        {
            return RedirectWithGoogleError(
                googleState.ReturnUrl,
                "ValidationError",
                exception.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid external login.");
        }
    }

    [HttpPost("send-password-recovery")]
    public async Task<IActionResult> PasswordRecovery(SendPasswordRecoveryCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("verify-password-code")]
    public async Task<IActionResult> VerifyPasswordCode(VerifyPasswordRecoveryCodeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("password-recovery")]
    public async Task<IActionResult> PasswordRecovery(PasswordRecoveryCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("refresh-user")]
    public async Task<IActionResult> RefreshUser(RefreshUserCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("user-exists")]
    public async Task<IActionResult> UserExists([FromQuery] UserExistsQuery command)
    {
        var result = await mediator.Send(command);

        if (result) return NoContent();
        return NotFound();
    }

    private IActionResult StartGoogleLogin(GoogleLoginApplication application, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(googleOptions.ClientId))
            return RedirectWithGoogleError(returnUrl, "GoogleNotConfigured", "Google authentication is not configured.");

        var normalizedReturnUrl = NormalizeFrontendReturnUrl(application, returnUrl);
        var state = Guid.NewGuid().ToString("N");
        var nonce = Guid.NewGuid().ToString("N");
        var payload = new GoogleOAuthState(state, nonce, normalizedReturnUrl, application);

        Response.Cookies.Append(
            GoogleStateCookie,
            Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload))),
            new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                MaxAge = TimeSpan.FromMinutes(10)
            });

        var url = QueryHelpers.AddQueryString("https://accounts.google.com/o/oauth2/v2/auth", new Dictionary<string, string?>
        {
            ["client_id"] = googleOptions.ClientId,
            ["redirect_uri"] = GoogleCallbackUrl(),
            ["response_type"] = "code",
            ["scope"] = "openid email profile",
            ["state"] = state,
            ["nonce"] = nonce,
            ["prompt"] = "select_account"
        });

        return Redirect(url);
    }

    private bool TryReadGoogleState(string? state, out GoogleOAuthState googleState)
    {
        googleState = default!;

        if (string.IsNullOrWhiteSpace(state) || !Request.Cookies.TryGetValue(GoogleStateCookie, out var cookieValue))
            return false;

        try
        {
            var json = Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(cookieValue));
            var payload = JsonSerializer.Deserialize<GoogleOAuthState>(json);

            if (payload is null || payload.State != state)
                return false;

            googleState = payload;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private RedirectResult RedirectWithGoogleAuth(string returnUrl, AuthDto response)
    {
        var payload = Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response, WebJsonOptions)));
        return Redirect(QueryHelpers.AddQueryString(returnUrl, "auth", payload));
    }

    private RedirectResult RedirectWithGoogleError(string? returnUrl, string code, string message)
    {
        var redirectUrl = QueryHelpers.AddQueryString(
            NormalizeFrontendReturnUrl(GoogleLoginApplication.Client, returnUrl),
            new Dictionary<string, string?>
            {
                ["error"] = code,
                ["message"] = message
            });

        return Redirect(redirectUrl);
    }

    private string GoogleCallbackUrl()
    {
        if (!string.IsNullOrWhiteSpace(googleOptions.PublicOrigin))
            return $"{googleOptions.PublicOrigin.TrimEnd('/')}/api/auth/google/callback";

        return Url.ActionLink(nameof(GoogleCallback)) ?? $"{Request.Scheme}://{Request.Host}/api/auth/google/callback";
    }

    private static string NormalizeFrontendReturnUrl(GoogleLoginApplication application, string? returnUrl)
    {
        var fallback = application == GoogleLoginApplication.Panel
            ? "http://localhost:5173/google-callback"
            : "http://localhost:4200/user/google-callback";

        if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri)
            || uri.Host != "localhost"
            || uri.Scheme is not ("http" or "https")
            || uri.Port is not (3000 or 3002 or 4200 or 5173))
        {
            return fallback;
        }

        return uri.ToString();
    }

    private sealed record GoogleOAuthState(
        string State,
        string Nonce,
        string ReturnUrl,
        GoogleLoginApplication Application);
}

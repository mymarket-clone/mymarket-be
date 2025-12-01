using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Common.Exceptions;

namespace Mymarket.WebApi.Infrastructure;

public class GlobalExceptionHandlerMiddleware(
    RequestDelegate _next,
    ILogger<GlobalExceptionHandlerMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = ex.Message,
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problem.Extensions["code"] = "UnauthorizedAccessError";

            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";

            var problem = new ValidationProblemDetails(
                ex.Errors
                   .GroupBy(f => f.PropertyName)
                   .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray())
            )
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Validation Failed",
                Status = StatusCodes.Status400BadRequest,
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problem.Extensions["code"] = "ValidationError";

            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (EmailNotVerifiedException ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Email not verified",
                Status = StatusCodes.Status401Unauthorized,
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problem.Extensions["email"] = ex.Email;
            problem.Extensions["code"] = "EmailNotVerified";

            await context.Response.WriteAsJsonAsync(problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "An unexpected error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Instance = $"{context.Request.Method} {context.Request.Path}"
            };

            problem.Extensions["code"] = "UnexpectedError";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}

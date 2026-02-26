using Microsoft.AspNetCore.Mvc;

namespace Mymarket.Domain.Constants;

public sealed class ApiValidationProblem : ProblemDetails
{
    public Dictionary<string, string[]> Errors { get; init; } = [];
    public string Code { get; init; } = "ValidationError";
}
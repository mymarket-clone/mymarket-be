namespace Mymarket.Application.Common.Exceptions;

public sealed class AttributeValidationException(Dictionary<string, string[]> errors) : Exception
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}

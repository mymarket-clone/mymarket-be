namespace Mymarket.Application.Common.Exceptions;

public class EmailNotVerifiedException(string email) : Exception
{
    public string Email { get; } = email;
}

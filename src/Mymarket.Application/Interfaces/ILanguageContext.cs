namespace Mymarket.Application.Interfaces;

public interface ILanguageContext
{
    string Language { get; }
    string Get(string? en, string? ru, string? fallback);
}

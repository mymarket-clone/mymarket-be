namespace Mymarket.Application.Interfaces;

public interface ILanguageContext
{
    string Language { get; }
    Func<T, string> LocalizeProperty<T>(string basePropertyName);
}

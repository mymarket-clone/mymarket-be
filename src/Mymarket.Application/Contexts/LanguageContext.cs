using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Contexts;

public class LanguageContext : ILanguageContext
{
    public string Language { get; }

    public LanguageContext(IHttpContextAccessor accessor)
    {
        var raw = accessor.HttpContext?
            .Request.Headers["Accept-Language"]
            .FirstOrDefault();

        Language = raw switch
        {
            "en" => "en",
            "ru" => "ru",
            _ => "ka"
        };
    }

    public string Get(string? en, string? ru, string? fallback)
    {
        return Language switch
        {
            "en" => en ?? fallback ?? String.Empty,
            "ru" => ru ?? fallback ?? String.Empty,
            _ => fallback ?? String.Empty
        };
    }
}
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
}
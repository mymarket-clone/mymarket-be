using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;
using System.Reflection;

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

    public Func<T, string> LocalizeProperty<T>(string basePropertyName)
    {
        return entity =>
        {
            if (entity == null) return null!;

            var propertyName = Language switch
            {
                "en" => basePropertyName + "En",
                "ru" => basePropertyName + "Ru",
                _ => basePropertyName
            };

            var prop = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
                throw new InvalidOperationException($"Property '{propertyName}' not found on type {typeof(T).Name}");

            var value = prop.GetValue(entity) as string;

            if (string.IsNullOrEmpty(value))
            {
                var baseProp = typeof(T).GetProperty(basePropertyName, BindingFlags.Public | BindingFlags.Instance);
                value = baseProp?.GetValue(entity) as string;
            }

            return value ?? string.Empty;
        };
    }
}
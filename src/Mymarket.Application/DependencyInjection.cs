using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mymarket.Application.Contexts;
using Mymarket.Application.Interfaces;
using System.Reflection;

namespace Mymarket.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var config = new MapperConfiguration(cfg =>
        {
            var dtoTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)));

            foreach (var dto in dtoTypes)
            {
                var instance = Activator.CreateInstance(dto);
                var method = dto.GetMethod("Mapping");
                method?.Invoke(instance, [cfg]);
            }
        });

        IMapper mapper = config.CreateMapper();

        builder.Services.AddSingleton(mapper);
        builder.Services.AddSingleton<IConfigurationProvider>(config);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ILanguageContext, LanguageContext>();
    }
}

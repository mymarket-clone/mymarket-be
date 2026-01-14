using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mymarket.Application.Contexts;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using System.Reflection;

namespace Mymarket.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ILanguageContext, LanguageContext>();
    }
}

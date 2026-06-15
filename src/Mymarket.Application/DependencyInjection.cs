using FluentValidation;
using Mapster;
using Mapster.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mymarket.Application.Contexts;
using Mymarket.Application.features.Users.Commands.RegisterUser;
using Mymarket.Application.Interfaces;
using System.Reflection;
using Mymarket.Application.Common.Behaviours;
using Mymarket.Application.Services;

namespace Mymarket.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ILanguageContext, LanguageContext>();
        builder.Services.AddScoped<IAuthSessionService, AuthSessionService>();
        builder.Services.AddScoped<IListingPricingService, ListingPricingService>();
        builder.Services.AddSingleton<IEmailNormalizer, EmailNormalizer>();
        builder.Services.AddMemoryCache();

        // Mapster
        TypeAdapterConfig.GlobalSettings
            .ScanInheritedTypes(Assembly.GetExecutingAssembly());

        // MediatR
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

        // Fluent validation
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
    }
}

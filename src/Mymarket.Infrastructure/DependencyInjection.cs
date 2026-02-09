using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Common;
using Mymarket.Application.features.Users.Commands.RegisterUser;
using Mymarket.Application.Interfaces;
using Mymarket.Infrastructure.Authentication;
using Mymarket.Infrastructure.Behaviours;
using Mymarket.Infrastructure.Data;
using Mymarket.Infrastructure.Services;
using Supabase;
using System.Text;

namespace Mymarket.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastuctureServices(this IHostApplicationBuilder builder)
    {
        // Database
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Supabase"));
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // MediatR
        builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

        // Problem details RFC 7807
        builder.Services.AddProblemDetails();

        // Fluent validation
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        // Jwt authentication
        builder.Services.AddSingleton<ITokenProvider, TokenProvider>();

        // Smtp
        builder.Services.AddSingleton<IEmailSender, EmailSender>();

        var jwtSettings = builder.Configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>() ?? throw new InvalidOperationException("JwtSettings missing");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.ClaimsIssuer = jwtSettings.Issuer;
                options.Audience = jwtSettings.Audience;
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

        // Supabase
        builder.Services.AddSingleton(provider =>
        {
            var config = builder.Configuration.GetSection("Supabase");

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var client = new Client(
                config["Url"]!,
                config["AnonKey"]!,
                options
            );

            client.InitializeAsync().GetAwaiter().GetResult();
            return client;
        });
    }
}

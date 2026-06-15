using EFCoreSecondLevelCacheInterceptor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Interfaces;
using Mymarket.Infrastructure.Authentication;
using Mymarket.Infrastructure.Authentication.Policies;
using Mymarket.Infrastructure.Data;
using Mymarket.Infrastructure.Services;
using Mymarket.Infrastructure.SignalR.Chat;
using Supabase;
using System.Text;

namespace Mymarket.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastuctureServices(this IHostApplicationBuilder builder)
    {
        // Database
        builder.Services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("Supabase"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            )
            .AddInterceptors(provider.GetRequiredService<SecondLevelCacheInterceptor>());
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddEFSecondLevelCache(options =>options.UseMemoryCacheProvider().UseCacheKeyPrefix("EF_") );

        // SignalR
        builder.Services.AddSignalR();

        builder.Services.AddTransient<IChatNotifier, ChatNotifier>();

        // Problem details RFC 7807
        builder.Services.AddProblemDetails();

        // Jwt authentication
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

        var jwtOptions = builder.Configuration
            .GetSection(nameof(JwtOptions))
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException("JwtOptions configuration is missing");

        builder.Services.AddSingleton(jwtOptions);

        var googleOptions = builder.Configuration
            .GetSection("Authentication:Google")
            .Get<GoogleOptions>()
            ?? new GoogleOptions();

        builder.Services.AddSingleton(googleOptions);

        // Smtp
        builder.Services.AddSingleton<IEmailSender, EmailSender>();

        // Custom services
        builder.Services.AddScoped<IImageService, ImageService>();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.ClaimsIssuer = jwtOptions.Issuer;
                options.Audience = jwtOptions.Audience;
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
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

        // Permissions 
        builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    }
}

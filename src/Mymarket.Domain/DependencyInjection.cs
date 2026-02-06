using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mymarket.Domain.Services;

namespace Mymarket.Domain;
public static class DependencyInjection
{
    public static void AddDomainServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<ImageService>();
    }
}

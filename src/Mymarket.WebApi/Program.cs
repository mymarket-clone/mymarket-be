using Mymarket.Application;
using Mymarket.Domain;
using Mymarket.Infrastructure;
using Mymarket.WebApi.Infrastructure;
using Mymarket.WebApi.Middlewares;

namespace Mymarket.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddApplicationServices();
        builder.AddDomainServices();
        builder.AddInfrastuctureServices();
        builder.AddWebApiServices();

        var app = builder.Build();

        app.UseMiddleware<SessionMiddleware>();

        app.ConfigureMiddleware();

        app.Run();
    }
}

namespace Mymarket.WebApi;

public static class DependencyInjection
{
    public static void AddWebApiServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                //var origins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>();
                //policy.WithOrigins(origins!)
                //    .AllowAnyMethod()
                //    .AllowAnyHeader();

                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
    }
}

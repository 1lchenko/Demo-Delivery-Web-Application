using System.Globalization;
using Demo_Delivery.Application.Options;
using Microsoft.AspNetCore.Localization;

namespace Demo_Delivery.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services
            .Configure<RequestLocalizationOptions>(options =>
            {
                var invariantCulture = CultureInfo.InvariantCulture;
                options.DefaultRequestCulture = new RequestCulture(invariantCulture);
                options.SupportedCultures = new List<CultureInfo> { invariantCulture };
                options.SupportedUICultures = new List<CultureInfo> { invariantCulture };
            })
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddCors(options =>
            {
                options.AddPolicy("PolicyApp",
                    build => build
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            })
            .AddControllers();


        return services;
    }
}
using Demo_Delivery.Infrastructure.Common.Persistence;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo_Delivery.Infrastructure.Common.Extension;

public static class InfrastructureExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication webApplication)
    {
        webApplication.UseMigration<ApplicationDbContext>(webApplication.Environment);
        return webApplication;
    }

    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app, IWebHostEnvironment env)
        where TContext : DbContext
    {
        //fourth
        var postgresOptions = app.ApplicationServices.GetRequiredService<PostgresSqlOptions>();
        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

        if (!env.IsEnvironment("test"))
        {
            SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }

        return app;
    }

    private static async Task<IServiceProvider> SeedDataAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var initializers = scope.ServiceProvider.GetServices<IInitializerDb>();
        foreach (var initializer in initializers) await initializer.InitializeAsync();

        return serviceProvider;
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }
}
using Demo_Delivery.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace Demo_Delivery.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services
            .Scan(scan =>
                scan.FromCallingAssembly()
                    .AddClasses(f => f.AssignableTo(typeof(IInicialData)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
    }
}
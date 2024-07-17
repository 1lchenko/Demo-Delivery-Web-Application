using Microsoft.Extensions.DependencyInjection;

namespace Demo_Delivery.Infrastructure.Common.Extension;

public static class ServiceCollectionExtensions
{
    public static void ReplaceSingleton<TService>(this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory) where TService : class
    {
        services.Unregister<TService>();
        services.AddSingleton(implementationFactory);
    }

    public static void Unregister<TService>(this IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
        services.Remove(descriptor);
    }
}
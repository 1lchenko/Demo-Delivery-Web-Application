using System.Reflection;
using Demo_Delivery.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo_Delivery.Application;

public static class ApplicationExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(cfg
                => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidateBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
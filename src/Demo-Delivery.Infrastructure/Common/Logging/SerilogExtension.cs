using System.Text;
using Demo_Delivery.Application.Options;
using Demo_Delivery.Application.Options.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SpectreConsole;

namespace Demo_Delivery.Infrastructure.Common.Logging;

public static class SerilogExtension
{
    public static IHostBuilder AddSerilog(this WebApplicationBuilder builder, IWebHostEnvironment environment)
    {
        return builder.Host.UseSerilog((context, provider, logConf) =>
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logOptions = context.Configuration.GetSection(LogOptions.Log).Get<LogOptions>()!;
            var appOptions = context.Configuration.GetSection(AppOptions.App).Get<AppOptions>()!;
            var logLevel = Enum.TryParse<LogEventLevel>(logOptions.Level, out var level)
                ? level
                : LogEventLevel.Information;

            logConf
                .MinimumLevel.Is(logLevel)
                .WriteTo.SpectreConsole(logOptions.LogTemplate, logLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(context.Configuration);

            if (logOptions.Elastic is { Enabled: true })
                logConf.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(logOptions.Elastic.ElasticServiceUrl))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = $"{appOptions.Name}-{env?.ToLower()}"
                    });

            if (logOptions.File is { Enabled: true })
            {
                var root = environment.ContentRootPath;
                var path = string.IsNullOrWhiteSpace(logOptions.File.Path) ? "logs/.txt" : logOptions.File.Path;
                if (!Enum.TryParse<RollingInterval>(logOptions.File.Interval, true, out var interval))
                    interval = RollingInterval.Day;

                logConf.WriteTo.File(path, rollingInterval: interval, encoding: Encoding.UTF8,
                    outputTemplate: logOptions.LogTemplate);
            }
        });
    }
}
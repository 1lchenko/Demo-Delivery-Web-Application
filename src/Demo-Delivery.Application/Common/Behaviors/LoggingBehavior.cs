using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Demo_Delivery.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        const string prefix = nameof(LoggingBehavior<TRequest, TResponse>);
        _logger.LogInformation("[{Prefix}] Handle request={Request} and response={Response}",
            prefix, typeof(TRequest).Name, typeof(TResponse).Name);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
            _logger.LogWarning("[{Perf-Possible}] The request {Request} took {TimeTaken} seconds",
                prefix, typeof(TRequest).Name, timeTaken.Seconds);

        _logger.LogInformation("[{Prefix}] Handled request {Request}",
            prefix, typeof(TRequest).Name);

        return response;
    }
}
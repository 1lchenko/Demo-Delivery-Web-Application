using Demo_Delivery.Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Demo_Delivery.Application.Common.Behaviors;

public class RequestValidateBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidateBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var errors = _validators
            .Select(async v => await v.ValidateAsync(context, cancellationToken))
            .SelectMany(r => r.Result.Errors)
            .Where(f => f != null)
            .ToList();

        if (errors.Count > 0) throw new ModelValidationException(errors);

        return await next();
    }
}
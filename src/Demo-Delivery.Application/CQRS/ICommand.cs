using MediatR;

namespace Demo_Delivery.Application.CQRS;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
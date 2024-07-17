using MediatR;

namespace Demo_Delivery.Application.CQRS;

public interface IQuery<out T> : IRequest<T>
{
}
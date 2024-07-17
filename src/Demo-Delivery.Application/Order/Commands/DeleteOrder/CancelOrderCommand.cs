using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;

namespace Demo_Delivery.Application.Order.Commands.DeleteOrder;

using Order = Domain.Entities.OrderAggregate.Order;

public record CancelOrderCommand(int OrderId) : ICommand;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand>
{
    private readonly IRepository<Order> _orderRepository;

    public CancelOrderCommandHandler(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        order.SetCancelledStatus();
        await _orderRepository.SaveChangesAsync(cancellationToken);
    }
}
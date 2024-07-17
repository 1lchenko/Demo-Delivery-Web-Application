using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Domain.Entities.OrderAggregate;

namespace Demo_Delivery.Application.Order.Commands.UpdateOrder;

public record UpdateOrderCommand(int Id, DateTime? WhenDeliver, bool DeliverForNearFuture, string Status, AddressDto Address) : ICommand;

public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
{
    private readonly IRepository<Domain.Entities.OrderAggregate.Order> _orderRepository;

    public UpdateOrderCommandHandler(IRepository<Domain.Entities.OrderAggregate.Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OrderAggregate.Order), request.Id);
        }

        var address = new Address(request.Address.Street, request.Address.IntercomPinCode,
            request.Address.BuildingNumber, request.Address.ApartmentNumber, request.Address.Note);
        order.UpdateOrder(request.Status, request.WhenDeliver, request.DeliverForNearFuture, address);

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
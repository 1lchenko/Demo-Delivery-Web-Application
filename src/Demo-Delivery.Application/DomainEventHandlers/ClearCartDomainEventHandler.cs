using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Domain.Events;
using MediatR;

namespace Demo_Delivery.Application.DomainEventHandlers;
using Cart = Domain.Entities.CartAggregate.Cart;

public class ClearCartDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
{
    private readonly IRepository<Cart> _cartRepository;

    public ClearCartDomainEventHandler(IRepository<Cart> cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var spec = new CartByCustomerIdSpecification(domainEvent.CustomerId);
        var cart = await _cartRepository.FirstOrDefaultAsync(spec, cancellationToken);
        //cart.ClearCart();
    }
}
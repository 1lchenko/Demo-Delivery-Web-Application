using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Domain.Entities.CustomerAggregate;
using Demo_Delivery.Domain.Events;
using MediatR;

namespace Demo_Delivery.Application.DomainEventHandlers;

using Cart = Domain.Entities.CartAggregate.Cart;

public class CreateCartAndCustomerWhenUserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<Customer> _customerRepository;

    public CreateCartAndCustomerWhenUserRegisteredDomainEventHandler(IRepository<Customer> customerRepository,
        IRepository<Cart> cartRepository)
    {
        _customerRepository = customerRepository;
        _cartRepository = cartRepository;
    }

    public async Task Handle(UserRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var customer = new Customer(domainEvent.UserName, domainEvent.UserId, domainEvent.Email);

        await _customerRepository.AddAsync(customer, cancellationToken);

        var cart = new Cart(customer.Id);

        await _cartRepository.AddAsync(cart, cancellationToken);
    }
}
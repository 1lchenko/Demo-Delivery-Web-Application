using Demo_Delivery.Application.Common.Abstractions.Services;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Cart.Commands.ClearCart;

using Cart = Domain.Entities.CartAggregate.Cart;

public record ClearCartCommand : ICommand;

public class ClearCartCommandHandler : ICommandHandler<ClearCartCommand>
{
    private readonly IRepository<Cart> _cartRepository;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IUser _currentUser;

    public ClearCartCommandHandler(IRepository<Cart> cartRepository, IUser currentUser, IReadRepository<Customer> customerRepository)
    {
        _cartRepository = cartRepository;
        _currentUser = currentUser;
        _customerRepository = customerRepository;
    }

    public async Task Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException($"User '{_currentUser.Id}' does not have an associated customer.");
        }
        var cartSpec = new CartByCustomerIdSpecification(customer.Id);
        var cart = await _cartRepository.FirstOrDefaultAsync(cartSpec, cancellationToken);
        
        if (cart is null)
        {
            throw new BadRequestException($"Customer '{customer.Id}' does not have an existing shopping cart.");
        }
        
        cart.ClearCart();
        await _cartRepository.SaveChangesAsync(cancellationToken);
    }
}
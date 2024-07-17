using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Cart.Queries.GetCustomerCartQuery;

public record GetCustomerCartQuery : IQuery<CartViewModel>;

public class GetCustomerCartQueryHandler : IQueryHandler<GetCustomerCartQuery, CartViewModel>
{
    private readonly IReadRepository<Domain.Entities.CartAggregate.Cart> _cartRepository;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IUser _currentUser;

    public GetCustomerCartQueryHandler(IReadRepository<Domain.Entities.CartAggregate.Cart> cartRepository, IUser currentUser, IReadRepository<Customer> customerRepository)
    {
        _cartRepository = cartRepository;
        _currentUser = currentUser;
        _customerRepository = customerRepository;
    }

    public async Task<CartViewModel> Handle(GetCustomerCartQuery request, CancellationToken cancellationToken)
    {
        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException($"User '{_currentUser.Id}' does not have an associated customer.");
        }

        var cartDtoSpec = new CartAsDtoByCustomerIdSpecification(customer.Id);
        var cartDto = await _cartRepository.FirstOrDefaultAsync(cartDtoSpec, cancellationToken);

        if (cartDto is null)
        {
            throw new BadRequestException($"Customer '{customer.Id}' does not have an existing shopping cart.");
        }

        return cartDto;
    }
}
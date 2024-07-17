using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CartAggregate;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Cart.Queries.GetCartItemInfo;

public record GetCartItemQuantityQuery(int ProductId) : IQuery<int>;


public class GetCartItemInfoQueryHandler : IQueryHandler<GetCartItemQuantityQuery, int>
{
    private readonly IReadRepository<CartItem> _cartItemRepository;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IUser _currentUser;

    public GetCartItemInfoQueryHandler(IReadRepository<CartItem> cartItemRepository, IReadRepository<Customer> customerRepository, IUser currentUser)
    {
        _cartItemRepository = cartItemRepository;
        _customerRepository = customerRepository;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(GetCartItemQuantityQuery request, CancellationToken cancellationToken)
    {
        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException($"User '{_currentUser.Id}' does not have an associated customer.");
        }

        var spec = new CartItemQuantityByCustomerAndProductId(customer.Id, request.ProductId);
        var cartItemDto = await _cartItemRepository.FirstOrDefaultAsync(spec, cancellationToken);
        return cartItemDto;
    }
}
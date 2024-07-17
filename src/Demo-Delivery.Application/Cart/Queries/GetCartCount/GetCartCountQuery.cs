using Demo_Delivery.Application.Common.Abstractions.Services;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Cart.Queries.GetCartCount;

public record GetCartCountQuery : IQuery<int>;

public class GetCartCountQueryHandler : IQueryHandler<GetCartCountQuery, int>
{
    private readonly IReadRepository<Domain.Entities.CartAggregate.Cart> _cartReadRepository;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IUser _currentUser;

    public GetCartCountQueryHandler(IUser currentUser,
        IReadRepository<Domain.Entities.CartAggregate.Cart> cartReadRepository,
        IReadRepository<Customer> customerRepository)
    {
        _currentUser = currentUser;
        _cartReadRepository = cartReadRepository;
        _customerRepository = customerRepository;
    }

    public async Task<int> Handle(GetCartCountQuery request, CancellationToken cancellationToken)
    {
        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException($"User '{_currentUser.Id}' does not have an associated customer.");
        }

         
        
        var spec = new CartCountByCustomerId(customer.Id);
        var cartCount = await _cartReadRepository.FirstOrDefaultAsync(spec, cancellationToken);
        return cartCount;
    }
}
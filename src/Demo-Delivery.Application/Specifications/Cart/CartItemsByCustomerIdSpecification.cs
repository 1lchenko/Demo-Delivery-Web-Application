using Ardalis.Specification;
using Demo_Delivery.Domain.Entities.CartAggregate;

namespace Demo_Delivery.Application.Specifications.Cart;

public class CartItemsByCustomerIdSpecification : Specification<CartItem>
{
    public CartItemsByCustomerIdSpecification(int customerId)
    {
        Query
            .Where(x => x.Cart.CustomerId == customerId);
    }  
}
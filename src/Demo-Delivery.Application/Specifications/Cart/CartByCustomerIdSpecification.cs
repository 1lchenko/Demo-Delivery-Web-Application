using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Cart;

public class CartByCustomerIdSpecification : Specification<Domain.Entities.CartAggregate.Cart>
{
    public CartByCustomerIdSpecification(int customerId)
    {
        Query
            .Where(cart => cart.CustomerId == customerId)
            .Include(x => x.CartItems.OrderBy(x => x.ProductName));

    }
}
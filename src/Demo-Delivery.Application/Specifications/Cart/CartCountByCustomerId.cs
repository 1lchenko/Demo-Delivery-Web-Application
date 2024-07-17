using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Cart;

public class CartCountByCustomerId : Specification<Domain.Entities.CartAggregate.Cart, int>
{
    public CartCountByCustomerId(int customerId)
    {
        Query
            .Select(x => x.GetCartItemsQuantity)
            .Include(x => x.CartItems)
            .Where(x => x.CustomerId == customerId);

    }
}
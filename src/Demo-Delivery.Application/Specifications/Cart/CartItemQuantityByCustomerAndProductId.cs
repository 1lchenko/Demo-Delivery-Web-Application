using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Domain.Entities.CartAggregate;

namespace Demo_Delivery.Application.Specifications.Cart;

public class CartItemQuantityByCustomerAndProductId :  Specification<CartItem, int>
{
    public CartItemQuantityByCustomerAndProductId(int customerId, int productId)
    {
        Query.Select(x => x.Quantity)
            .Where(x => x.Cart.CustomerId == customerId && x.ProductId == productId);
    }
}
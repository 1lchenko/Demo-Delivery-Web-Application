using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Domain.Entities.CartAggregate;

namespace Demo_Delivery.Application.Specifications.Cart;

public class CartItemAsDtoByCustomerId : Specification<CartItem, CartItemViewModel>
{
    public CartItemAsDtoByCustomerId(int customerId)
    {
        Query.Select(x => new CartItemViewModel
        {
            ProductId = x.ProductId,
            Quantity = x.Quantity
        }).Where(x => x.Cart.CustomerId == customerId);
    }
}
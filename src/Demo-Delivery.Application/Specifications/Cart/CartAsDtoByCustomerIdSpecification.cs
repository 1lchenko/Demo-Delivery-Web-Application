using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Cart;

namespace Demo_Delivery.Application.Specifications.Cart;

public class CartAsDtoByCustomerIdSpecification : Specification<Domain.Entities.CartAggregate.Cart, CartViewModel>
{
    public CartAsDtoByCustomerIdSpecification(int customerId)
    {
        Query.Select(x => new CartViewModel
            {
                CustomerId = x.CustomerId,
                CartItems = x.CartItems.OrderBy(x => x.ProductName).Select(y => new CartItemViewModel
                    {
                        ProductId = y.ProductId,
                        ProductName = y.ProductName,
                        Price = y.UnitPrice,
                        ImageKey = y.ImageKey,
                        Quantity = y.Quantity
                    })
                    .ToList()
                    
            })
            .Where(x => x.CustomerId == customerId);
    }
}
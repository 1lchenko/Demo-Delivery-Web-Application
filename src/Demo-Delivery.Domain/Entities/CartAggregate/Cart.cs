using System.ComponentModel.DataAnnotations.Schema;
using Demo_Delivery.Domain.Entities.ProductAggregate;
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.CartAggregate;

public class Cart : Entity, IAggregateRoot
{
    public int CustomerId { get; private set; }
    public int CartQuantity => _cartItems.Sum(x => x.Quantity);

    private readonly List<CartItem> _cartItems = new();
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public Cart(int customerId)
    {
        CustomerId = customerId;
        
    }

    protected Cart()
    {
         
    }

    public void AddCartItem(Product product)
    {
        var existCartItem = _cartItems.FirstOrDefault(x => x.ProductId == product.Id);
        if (existCartItem is not null)
        {
            existCartItem.UpdateQuantity(existCartItem.Quantity + 1);
        }
        else
        {
            var cartItem = new CartItem(this, product.Id, product.Name, product.Price,
                product.ImageKeys.FirstOrDefault(), 1);
            _cartItems.Add(cartItem);
        }
    }

    public void UpdateCartItemQuantity(int productId, int quantity)
    {
        
        var existCartItem = _cartItems.FirstOrDefault(x => x.ProductId == productId);
        if (existCartItem is not null)
        {
            if (quantity == 0)
            {
                RemoveCartItem(productId);
            }
            else
            {
                existCartItem.UpdateQuantity(quantity);
            }

        }
        else
        {
            throw new CartDomainException("This product does not exist in the cart.");
        }
    }

    public void RemoveCartItem(int productId)
    {
        _cartItems.RemoveAll(x => x.ProductId == productId);
    }

    public void ClearCart()
    {
        _cartItems.Clear();
    }

    [NotMapped] public int GetCartItemsQuantity => _cartItems.Sum(x => x.Quantity);

    [NotMapped] public decimal GetTotalCartPrice => _cartItems.Sum(x => x.UnitPrice * x.Quantity);
}
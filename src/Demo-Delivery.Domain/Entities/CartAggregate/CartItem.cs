using System.ComponentModel.DataAnnotations.Schema;
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.CartAggregate;

using static DomainConstants.CartItemValidationConstants;

public class CartItem : Entity
{
    public int CartId { get; private set; }
    public int ProductId { get; private set; }

    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? ImageKey { get; private set; }
    public int Quantity { get; private set; }

    public Cart Cart { get; private set; }

    [NotMapped] public decimal TotalPrice => Quantity * UnitPrice;

    public CartItem(Cart cart, int productId, string productName, decimal unitPrice, string? imageKey, int quantity)
    {
        Cart = cart;
        CartId = cart.Id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        ImageKey = imageKey;
        Quantity = quantity;
    }

    protected CartItem()
    {
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
    
}
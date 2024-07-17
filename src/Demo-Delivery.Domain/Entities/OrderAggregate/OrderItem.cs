using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.OrderAggregate;

public class OrderItem : Entity
{
    protected OrderItem()
    {
        
    }
    public OrderItem(int productId, string productName, int quantity, decimal unitPrice, string? productImageKey)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        ImageKey = productImageKey;
    }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public int OrderId { get; private set; }
    public int ProductId { get; private set; }

    public string ProductName { get; private set; }
    public string? ImageKey { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public decimal TotalPrice => Quantity * UnitPrice;
}
using Demo_Delivery.Domain.Entities.CartAggregate;
using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Domain.Entities.OrderAggregate;
using Demo_Delivery.Domain.Entities.ProductAggregate;

namespace Demo_Delivery.UnitTests;

public class AddressBuilder
{
    public Address Build()
    {
        return new Address("street", "123", "12A", 2, "note");
    }
}

public class OrderBuilder
{
    private readonly Order _order;

    public OrderBuilder(Address address)
    {
        _order = new Order(1, "comment", null, true, address);
    }

    public OrderBuilder(Address address, DateTime? whenDeliver)
    {
        _order = whenDeliver.HasValue
            ? new Order(1, "comment", whenDeliver.Value, false, address)
            : new Order(1, "comment", null, true, address);
    }

    public OrderBuilder AddOrderItem(int productId, string productName, decimal unitPrice, string? productImageKey,
        int quantity)
    {
        _order.AddOrderItem(productId, productName, unitPrice, productImageKey, quantity);
        return this;
    }

    public Order Build()
    {
        return _order;
    }
}

public class CartBuilder
{
    private readonly Cart _cart;

    public CartBuilder()
    {
        _cart = new Cart(1);
    }

    public Cart Build()
    {
        return _cart;
    }
}

public class ProductBuilder
{
    private readonly Product _product;

    public ProductBuilder(int? amountOnStock = null)
    {
        var category = new Category("name");
        _product = new Product("name", "description", 1.0m, 1, amountOnStock ?? 1, 1,
            true, category);
    }

    public Product Build()
    {
        return _product;
    }
}
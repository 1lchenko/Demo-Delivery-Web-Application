 

namespace Demo_Delivery.UnitTests.Domain;

public class CartAggregateTest
{
    [Fact]
    public void Update_cart_item_quantity_when_product_is_nonexist_throw_exception()
    {
        var cart = new CartBuilder().Build();

        const int nonexistProductId = 0;
        const int quantity = 10;
        void Act() => cart.UpdateCartItemQuantity(nonexistProductId, quantity);

        Assert.Throws<CartDomainException>(Act);
    }
}
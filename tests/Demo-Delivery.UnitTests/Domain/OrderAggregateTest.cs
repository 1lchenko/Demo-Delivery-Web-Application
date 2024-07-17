 

namespace Demo_Delivery.UnitTests.Domain;

public class OrderAggregateTest
{
    [Fact]
    public void When_add_two_times_on_the_same_item_then_the_total_of_order_should_be_the_sum_of_the_two_items()
    {
        var address = new AddressBuilder().Build();
        var order = new OrderBuilder(address)
            .AddOrderItem(1, "productName", 5.0m, null, 1)
            .AddOrderItem(1, "productName", 5.0m, null, 1)
            .Build();
        
        Assert.Equal(10.0m, order.FinalPrice);
    }

    [Fact]
    public void When_set_incorrect_order_status_throw_exception()
    {
        var address = new AddressBuilder().Build();
        var order = new OrderBuilder(address).Build();

        void Act() => order.SetPaidStatus();

        Assert.Throws<OrderingDomainException>(Act);


    }
}
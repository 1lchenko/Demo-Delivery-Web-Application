

namespace Demo_Delivery.UnitTests.Domain;

public class ProductAggregateTest
{
    [Fact]
    public void Add_more_than_three_image_keys_should_throw_exception()
    {
        var product = new ProductBuilder().Build();
        var imageKeys = new List<string> { "first", "second", "third", "fourth" };

        Assert.Throws<CatalogDomainException>(() => product.AddImageKeys(imageKeys));
    }

    [Fact]
    public void Remove_from_stock_when_amount_on_stock_is_zero_should_throw_exception()
    {
        var product = new ProductBuilder(0).Build();
        Assert.Throws<CatalogDomainException>(() => product.RemoveStock(1));
    }
}
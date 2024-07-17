using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Domain.Entities.ProductAggregate;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.EntityData;

public class ProductData : IInicialData
{
    public Type EntityType => typeof(Product);

    public IEnumerable<object> GetData()
    {

        var images = new string[]
        {
            "product-images/meal_firts.jpeg",
            "product-images/meal_second.jpeg",
            "product-images/meal_third.jpeg",
            "product-images/meal_fourth.jpeg",
            "product-images/meal_fifth.jpeg",
            "product-images/meal_eighth.jpeg",
            "product-images/meal_seventh.jpeg",
        };
        
        
        var products = new List<Product>();

        var defaultCategory = new Category("Pizza");
        var defaultCategory2 = new Category("Main");

        for (int i = 1; i <= 100; i++)
        {
            var product = new Product($"Product {i}", $"Description for Product {i}", decimal.Round(10m + i, 2),
                Random.Shared.Next(1, 9999), Random.Shared.Next(1, 9999), Random.Shared.Next(1, 9999), true,
                i <= 10 ? defaultCategory : defaultCategory2,  images.Skip(Random.Shared.Next(1,6)).Take(3).ToList());

             
            products.Add(product);
        }

        return products;
    }
}
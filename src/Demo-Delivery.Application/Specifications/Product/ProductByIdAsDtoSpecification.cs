using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Product;

namespace Demo_Delivery.Application.Specifications.Product;

using Product = Domain.Entities.ProductAggregate.Product;
public class ProductByIdAsDtoSpecification : Specification<Product, ProductDetailedViewModel>
{
    public ProductByIdAsDtoSpecification(int id)
    {
        Query
         .Select(x => new ProductDetailedViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                AmountOnStock = x.AmountOnStock,
                Calories = x.Calories,
                CategoryName = x.Category.Name,
                CategoryId = x.Category.Id,
                ImagesKeys = x.ImageKeys,
                Price = x.Price,
                IsActive = x.IsActive
            })
            .Where(x => x.Id == id)
            .AsNoTracking();
    }
}
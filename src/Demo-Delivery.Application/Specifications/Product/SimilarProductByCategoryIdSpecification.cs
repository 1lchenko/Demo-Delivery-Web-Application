using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Product;

namespace Demo_Delivery.Application.Specifications.Product;

public class SimilarProductByCategoryIdSpecification : Specification<Domain.Entities.ProductAggregate.Product, ProductViewModel>
{
    public SimilarProductByCategoryIdSpecification(int categoryId)
    {
        Query.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                AmountOnStock = p.AmountOnStock,
                Calories = p.Calories,
                CategoryName = p.Category.Name,
                ImagesKeys = p.ImageKeys,
                Price = p.Price
            })
            .Where(x => x.Category.Id == categoryId)
            .Take(5)
            .AsNoTracking();

    }
}
using Ardalis.Specification;
using Demo_Delivery.Application.Common.Sort;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Product.Queries.Common;

namespace Demo_Delivery.Application.Specifications.Product;

public class ProductsAsSummaryDtoSpecification : Specification<Domain.Entities.ProductAggregate.Product, AdminProductViewModel>
{
    public ProductsAsSummaryDtoSpecification(int take, int skip, string? search, SortProducts sort,
        int? categoryId)
    {
        Query.Select(product => new AdminProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name,
                Calories = product.Calories,
                AmountOnStock = product.AmountOnStock,
                ImagesKeys = product.ImageKeys
            })
            .Where(x => x.Category.Id == categoryId.Value, categoryId.HasValue)
            .Where(x => x.Name == search, !string.IsNullOrWhiteSpace(search))
            .ApplyOrdering(sort)
            .Skip(skip)
            .Take(take);
    }
}
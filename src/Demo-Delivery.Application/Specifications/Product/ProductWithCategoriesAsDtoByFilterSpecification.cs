using System.Text.RegularExpressions;
using Ardalis.Specification;
using Demo_Delivery.Application.Common.Sort;
using Demo_Delivery.Application.Dtos.Product;
using Microsoft.EntityFrameworkCore;

namespace Demo_Delivery.Application.Specifications.Product;

using Product = Domain.Entities.ProductAggregate.Product;

public class ProductWithCategoriesAsDtoByFilterSpecification : Specification<Product, ProductViewModel>
{
    public ProductWithCategoriesAsDtoByFilterSpecification(int take, int skip, string? search, SortBase<Product> sort,
        int? categoryId)
    {
        Query.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                AmountOnStock = x.AmountOnStock,
                Calories = x.Calories,
                CategoryName = x.Category.Name,
                ImagesKeys = x.ImageKeys,
                Price = x.Price
            })
            .Where(x => x.Name.Contains(search!), search is not null)
            .Where(x => x.Category.Id == categoryId!.Value, categoryId.HasValue)
            .ApplyOrdering(sort)
            .Skip(skip)
            .Take(take)
            .AsNoTracking();
    }
}
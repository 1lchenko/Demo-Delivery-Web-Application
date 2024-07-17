using Ardalis.Specification;
using Demo_Delivery.Application.Common.Sort;

namespace Demo_Delivery.Application.Specifications.Product;

using Product = Domain.Entities.ProductAggregate.Product;

public static class ProductSpecExtensions
{
    public static ISpecificationBuilder<Product> ApplyOrdering(this ISpecificationBuilder<Product> builder,
        SortBase<Product> sort)
    {
        if (!string.IsNullOrWhiteSpace(sort.SortBy))
        {
            builder = sort.OrderBy == SortBase<Product>.Ascending
                ? builder.OrderBy(sort.ToExpression())
                : builder.OrderByDescending(sort.ToExpression());
        }
         

        return builder;
    }
}
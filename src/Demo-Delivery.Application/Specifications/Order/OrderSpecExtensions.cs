using Ardalis.Specification;
using Demo_Delivery.Application.Common.Sort;

namespace Demo_Delivery.Application.Specifications.Order;

using Order = Domain.Entities.OrderAggregate.Order;

public static class OrderSpecExtensions
{
    public static ISpecificationBuilder<Order> ApplyOrdering(this ISpecificationBuilder<Order> builder,
        SortBase<Order> sort)
    {
        if (!string.IsNullOrWhiteSpace(sort.SortBy))
        {
            builder = sort.SortBy == SortBase<Order>.Ascending
                ? builder.OrderBy(sort.ToExpression())
                : builder.OrderByDescending(sort.ToExpression());
        }

        return builder;
    }
}
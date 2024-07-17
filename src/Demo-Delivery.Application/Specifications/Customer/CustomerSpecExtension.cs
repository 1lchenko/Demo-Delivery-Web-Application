using Ardalis.Specification;
using Demo_Delivery.Application.Common.Sort;

namespace Demo_Delivery.Application.Specifications.Customer;

public static class OrderSpecExtension
{
    public static ISpecificationBuilder<Domain.Entities.CustomerAggregate.Customer> ApplyOrdering(this ISpecificationBuilder<Domain.Entities.CustomerAggregate.Customer> builder,
        SortBase<Domain.Entities.CustomerAggregate.Customer> sort)
    {
        if (!string.IsNullOrWhiteSpace(sort.SortBy))
        {
            builder = sort.OrderBy == SortBase<Domain.Entities.CustomerAggregate.Customer>.Ascending
                ? builder.OrderBy(sort.ToExpression())
                : builder.OrderByDescending(sort.ToExpression());
        }

        return builder;
    }
}
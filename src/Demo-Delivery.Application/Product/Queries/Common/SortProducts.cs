using System.Linq.Expressions;
using Demo_Delivery.Application.Common.Sort;

namespace Demo_Delivery.Application.Product.Queries.Common;

public class SortProducts : SortBase<Domain.Entities.ProductAggregate.Product>
{
    public SortProducts(string? sortBy, string? orderBy) : base(sortBy, orderBy)
    {
    }

    public override Expression<Func<Domain.Entities.ProductAggregate.Product, object>> ToExpression()
        => this.SortBy switch
        {
            "price" => product => product.Price,
            "name" => product => product.Name,
            _ => product => product.Id
        };
}
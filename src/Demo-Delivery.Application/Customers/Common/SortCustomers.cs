using System.Linq.Expressions;
using Demo_Delivery.Application.Common.Sort;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Customers.Common;

public class SortCustomers : SortBase<Customer>
{
    public SortCustomers(string? sortBy, string? orderBy) : base(sortBy, orderBy)
    {
    }

    public override Expression<Func<Customer, object>> ToExpression() =>
        this.SortBy switch
        {
            "name" => customer => customer.Name,
            "lastPurchaseDate" => customer => customer.LastPurchaseDateUtc.Value,
            "lastCartUpdateDate" => customer => customer.LastUpdateCartDateUtc.Value,
            _ => product => product.Id
        };
}
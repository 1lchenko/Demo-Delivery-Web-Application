using System.Linq.Expressions;
using Demo_Delivery.Application.Common.Sort;

namespace Demo_Delivery.Application.Order.Common;

public class SortOrders : SortBase<Domain.Entities.OrderAggregate.Order>
{
    public SortOrders(string? sortBy, string? orderBy) : base(sortBy, orderBy)
    {
    }

    public override Expression<Func<Domain.Entities.OrderAggregate.Order, object>> ToExpression() =>
        this.SortBy switch
        {
            "status" => order => order.OrderStatus.Name,
            "date" => product => product.CreatedDate,
            _ => product => product.Id
        };
}
using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Order.Common;

namespace Demo_Delivery.Application.Specifications.Order;

using Order = Domain.Entities.OrderAggregate.Order;

public class OrdersByCustomerIdAndFilterAsDtoSpecification : Specification<Order, OrderViewModel>
{
    public OrdersByCustomerIdAndFilterAsDtoSpecification(int id, int take, int skip, string? search, SortOrders sortOrders,
        string? status, DateTime? fromDate, DateTime? toDate)
    {
        Query 
            .Select(x => new OrderViewModel()
            {
                Id = x.Id.ToString(),
                CreateOn = x.CreatedDate,
                Status = x.OrderStatus.ToString(),
                Discount = x.Discount,
                BaseSum = x.BasePrice,
                FinalSum = x.FinalPrice,
                OrderItems =
                    x.OrderItems.Select(item =>
                            new OrderItemDto { ProductName = item.ProductName, ImageKey = item.ImageKey })
                        .ToList()
            })
            .Include(x => x.OrderItems)
            .Where(x => x.CustomerId == id)
            .Where(x => x.OrderItems.Any(x => x.ProductName.Trim().ToLower() == search!.Trim().ToLower()),
                search is not null)
            .Where(o => o.CreatedDate >= fromDate.Value.ToUniversalTime() && o.CreatedDate <= toDate.Value.ToUniversalTime(),
                fromDate.HasValue && toDate.HasValue)
            .Where(o => o.OrderStatus.Name.Trim().ToLower() == status!.Trim().ToLower(), status is not null)
            .ApplyOrdering(sortOrders)
            .Skip(skip)
            .Take(take)
            .AsNoTracking();
    }
}
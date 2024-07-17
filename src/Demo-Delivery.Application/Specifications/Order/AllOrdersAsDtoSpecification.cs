using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Order.Common;

namespace Demo_Delivery.Application.Specifications.Order;

public class
    AllOrdersAsDtoSpecification : Specification<Domain.Entities.OrderAggregate.Order, CustomerOrderViewModel>
{
    public AllOrdersAsDtoSpecification(int? orderId, int? customerId, int take, int skip, string? search, SortCustomerOrders sortOrders,
        string? status, DateTime? fromDate, DateTime? toDate)
    {
        Query.Select(x => new CustomerOrderViewModel
            {
                Id = x.Id,
                BasePrice = x.BasePrice,
                CustomerId = x.CustomerId,
                Discount = x.Discount,
                FinalPrice = x.FinalPrice,
                HasVoucher = x.HasVoucher,
                WhenDeliver = x.WhenDeliver,
                DeliverForNearFuture = x.DeliverForNearFuture,
                CreatedOn = x.CreatedDate,
                Status = x.OrderStatus.ToString(),
                OrderItems =
                    x.OrderItems.Select(t => new OrderItemDto()
                        {
                            ImageKey = t.ImageKey,
                            ProductName = t.ProductName,
                        })
                        .ToList()
            })
            .Where(x => x.Id == orderId.Value, orderId.HasValue)
            .Where(x => x.OrderItems.Any(x => x.ProductName.Trim().ToLower() == search!.Trim().ToLower()),
                search is not null)
            .Where(o => o.CreatedDate >= fromDate.Value.ToUniversalTime(), fromDate.HasValue)
            .Where(o => o.CreatedDate <= toDate.Value.ToUniversalTime(), toDate.HasValue)
            .Where(o => o.OrderStatus.Name.Trim().ToLower() == status!.Trim().ToLower(), status is not null)
            .Where(o => o.CustomerId == customerId!.Value, customerId.HasValue)
            .ApplyOrdering(sortOrders)
            .Skip(skip)
            .Take(take)
            .AsNoTracking();
    }
}
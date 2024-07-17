using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Order.Common;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Application.Specifications.Order;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Order.Queries.GetAllOrder;

public record GetAllOrderQuery(string? Status, DateTime? FromDate, DateTime? ToDate, int? OrderId, int? CustomerId)
    : SortAndFilterBaseQuery<PaginatedList<CustomerOrderViewModel>>
{
    public const int PageSize = 10;
}

public class GetAllOrderQueryHandler : IQueryHandler<GetAllOrderQuery, PaginatedList<CustomerOrderViewModel>>
{
    private readonly IReadRepository<Domain.Entities.OrderAggregate.Order> _orderReadRepository;
    private readonly IReadRepository<Customer> _customerReadRepository;

    public GetAllOrderQueryHandler(IReadRepository<Domain.Entities.OrderAggregate.Order> orderReadRepository,
        IReadRepository<Customer> customerReadRepository)
    {
        _orderReadRepository = orderReadRepository;
        _customerReadRepository = customerReadRepository;
    }

    public async Task<PaginatedList<CustomerOrderViewModel>> Handle(GetAllOrderQuery request,
        CancellationToken cancellationToken)
    {
        var sortCustomerOrders = new SortCustomerOrders(request.sortBy, request.orderBy);
        
        var orderSpec = new AllOrdersAsDtoSpecification(request.OrderId, request.CustomerId ,GetAllOrderQuery.PageSize,
            (request.currentPage - 1) * GetAllOrderQuery.PageSize, request.search, sortCustomerOrders, request.Status,
            request.FromDate, request.ToDate);
        
        var orders = await _orderReadRepository.ListAsync(orderSpec, cancellationToken);

        if (orders.Any())
        {
            var customerSpec = new CustomersByIdSpecification(orders.Select(x => x.CustomerId).ToList());
            var customers = await _customerReadRepository.ListAsync(customerSpec, cancellationToken);

            foreach (var order in orders)
            {
                var customer = customers.FirstOrDefault(x => x.Id == order.CustomerId);
                if (customer != null)
                {
                    order.CustomerName = customer.Name;
                }
            }
        }

        var orderCount = orders.Any() ? await _orderReadRepository.CountAsync(orderSpec, cancellationToken) : 0;

        return new PaginatedList<CustomerOrderViewModel>(orders, request.currentPage, GetAllOrderQuery.PageSize,
            orderCount);
    }
}
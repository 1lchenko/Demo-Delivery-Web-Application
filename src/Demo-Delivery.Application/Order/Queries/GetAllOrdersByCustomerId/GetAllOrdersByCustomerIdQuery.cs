using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Order.Common;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Application.Specifications.Order;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Order.Queries.GetAllOrdersByCustomerId;

using Order = Domain.Entities.OrderAggregate.Order;

public record GetAllOrdersByCustomerIdQuery(string? Status, DateTime? FromDate, DateTime? ToDate)
    : SortAndFilterBaseQuery<PaginatedList<OrderViewModel>> 
{
    public const int PageSize = 10;
}

public class GetAllOrdersByCustomerIdQueryHandler : IQueryHandler<GetAllOrdersByCustomerIdQuery, PaginatedList<OrderViewModel>>
{
    private readonly IUser _currentUser;
    private readonly IReadRepository<Order> _orderRepository;
    private readonly IReadRepository<Customer> _customerRepository;

    public GetAllOrdersByCustomerIdQueryHandler(IUser currentUser, IReadRepository<Order> orderRepository,
        IReadRepository<Customer> customerRepository)
    {
        _currentUser = currentUser;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<PaginatedList<OrderViewModel>> Handle(GetAllOrdersByCustomerIdQuery request,
        CancellationToken cancellationToken)
    {
        var sortOrders = new SortOrders(request.sortBy, request.orderBy);
        
        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException("Order cannot be created. Customer does not exist.");
        }
        
        var ordersSpec = new OrdersByCustomerIdAndFilterAsDtoSpecification(customer.Id,
            GetAllOrdersByCustomerIdQuery.PageSize, (request.currentPage - 1) * GetAllOrdersByCustomerIdQuery.PageSize,
            request.search, sortOrders, request.Status, request.FromDate, request.ToDate);
        var totalCount = await _orderRepository.CountAsync(cancellationToken);
        var orders = await _orderRepository.ListAsync(ordersSpec, cancellationToken);

        return new PaginatedList<OrderViewModel>(orders, request.currentPage, GetAllOrdersByCustomerIdQuery.PageSize,
            totalCount);
    }
}
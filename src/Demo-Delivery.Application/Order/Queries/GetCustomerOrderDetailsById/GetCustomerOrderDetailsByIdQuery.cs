using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Specifications.Order;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Order.Queries.GetCustomerOrderDetailsById;

public record GetCustomerOrderDetailsByIdQuery(int OrderId) : IQuery<CustomerOrderDetailedViewModel>;

public class GetCustomerOrderDetailsByIdQueryHandler : IQueryHandler<GetCustomerOrderDetailsByIdQuery, CustomerOrderDetailedViewModel>
{
    private readonly IReadRepository<Domain.Entities.OrderAggregate.Order> _orderReadRepository;
    private readonly IReadRepository<Customer> _customerReadRepository;

    public GetCustomerOrderDetailsByIdQueryHandler(
        IReadRepository<Domain.Entities.OrderAggregate.Order> orderReadRepository,
        IReadRepository<Customer> customerReadRepository)
    {
        _orderReadRepository = orderReadRepository;
        _customerReadRepository = customerReadRepository;
    }

    public async Task<CustomerOrderDetailedViewModel> Handle(GetCustomerOrderDetailsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new CustomerOrderDetailedByIdSpecification(request.OrderId);
        var order = await _orderReadRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.OrderAggregate.Order), request.OrderId);
        }

        var customer = await _customerReadRepository.GetByIdAsync(order.CustomerId, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException("Order has no customer assigned. ");
        }
        
        order.CustomerName = customer.Name;

        return order;
    }
}
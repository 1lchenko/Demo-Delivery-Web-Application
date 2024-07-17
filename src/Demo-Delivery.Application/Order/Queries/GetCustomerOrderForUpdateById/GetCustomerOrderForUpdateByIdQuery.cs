using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Specifications.Order;

namespace Demo_Delivery.Application.Order.Queries.GetCustomerOrderForUpdateById;

public record GetCustomerOrderForUpdateByIdQuery(int OrderId) : IQuery<CustomerOrderForUpdateDto>;

public class GetCustomerOrderForUpdateByIdQueryHandler : IQueryHandler<GetCustomerOrderForUpdateByIdQuery, CustomerOrderForUpdateDto>
{
    private readonly IReadRepository<Domain.Entities.OrderAggregate.Order> _orderRepository;

    public GetCustomerOrderForUpdateByIdQueryHandler(IReadRepository<Domain.Entities.OrderAggregate.Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CustomerOrderForUpdateDto> Handle(GetCustomerOrderForUpdateByIdQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new CustomerOrderForUpdateById(request.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        return order;
    }
}
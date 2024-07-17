using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Specifications.Order;
using Demo_Delivery.Domain;

namespace Demo_Delivery.Application.Order.Queries.GetOrderById;

using Order = Domain.Entities.OrderAggregate.Order;

public record GetOrderByIdQuery(int OrderId) : IQuery<OrderDetailedViewModel>;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDetailedViewModel>
{
    private readonly IUser _currentUser;
    private readonly IReadRepository<Order> _repository;

    public GetOrderByIdQueryHandler(IUser currentUser, IReadRepository<Order> repository)
    {
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<OrderDetailedViewModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new OrderByIdWithItemsAsDtoSpecification(orderId: request.OrderId,
            userIsAdmin: _currentUser.HasRole(GlobalConstants.Roles.AdminRoleName));

        var orderDetailedDto = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (orderDetailedDto is null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        return orderDetailedDto;
    }
}
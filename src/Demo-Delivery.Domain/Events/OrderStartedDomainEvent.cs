
using Demo_Delivery.Domain.Entities.OrderAggregate;
using MediatR;

namespace Demo_Delivery.Domain.Events;

public record OrderStartedDomainEvent(Order Order, int CustomerId) : INotification;
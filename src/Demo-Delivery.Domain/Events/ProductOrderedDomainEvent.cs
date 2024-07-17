using MediatR;

namespace Demo_Delivery.Domain.Events;

public record ProductOrderedDomainEvent(Guid ProductId, int Quantity) : INotification;
 
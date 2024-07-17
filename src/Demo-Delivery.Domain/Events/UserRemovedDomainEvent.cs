using MediatR;

namespace Demo_Delivery.Domain.Events;

public record UserRemovedDomainEvent(string UserId) : INotification;
 
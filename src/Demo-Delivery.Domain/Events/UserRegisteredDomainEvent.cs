using MediatR;

namespace Demo_Delivery.Domain.Events;

public record UserRegisteredDomainEvent(string UserId, string UserName, string Email) : INotification;

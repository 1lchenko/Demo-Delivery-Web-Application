using Demo_Delivery.Domain.Entities.ProductAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Domain.Events;

public record ProductCreatedDomainEvent(Product Product, IEnumerable<IFormFile> ImageFiles) : INotification;
 
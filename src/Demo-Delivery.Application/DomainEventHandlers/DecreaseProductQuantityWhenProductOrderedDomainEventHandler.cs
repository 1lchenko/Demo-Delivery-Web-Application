using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Domain.Events;
using MediatR;

namespace Demo_Delivery.Application.DomainEventHandlers;

using Product = Domain.Entities.ProductAggregate.Product;

public class DecreaseProductQuantityWhenProductOrderedDomainEventHandler : INotificationHandler<ProductOrderedDomainEvent>
{
    private readonly IReadRepository<Product> _productReadRepository;

    public DecreaseProductQuantityWhenProductOrderedDomainEventHandler(IReadRepository<Product> productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task Handle(ProductOrderedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByIdAsync(domainEvent.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), domainEvent.ProductId);
        }

        product.RemoveStock(domainEvent.Quantity);
        // why need call savechanges?
    }
}
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;

namespace Demo_Delivery.Application.Product.Commands.DeleteProduct;

public record DeleteProductCommand(int ProductId) : ICommand;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IRepository<Domain.Entities.ProductAggregate.Product> _productRepository;

    public DeleteProductCommandHandler(IRepository<Domain.Entities.ProductAggregate.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.ProductAggregate.Product), request.ProductId);
        }

        await _productRepository.DeleteAsync(product, cancellationToken);
    }
}
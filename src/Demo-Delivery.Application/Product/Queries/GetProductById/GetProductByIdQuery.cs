using System.Data;
using Dapper;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Specifications.Product;

namespace Demo_Delivery.Application.Product.Queries.GetProductById;

public record GetProductByIdQuery(int ProductId) : IQuery<ProductDetailedViewModel>;

public class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDetailedViewModel>
{
    private readonly IReadRepository<Domain.Entities.ProductAggregate.Product> _productRepository;

    public GetProductByIdHandler(IReadRepository<Domain.Entities.ProductAggregate.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDetailedViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductByIdAsDtoSpecification(request.ProductId);
        var productDetailed = await _productRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (productDetailed is null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }
        
        productDetailed.SimilarProducts = await GetSimilarProducts(productDetailed.CategoryId, cancellationToken);

        return productDetailed;
    }

    private async Task<List<ProductViewModel>> GetSimilarProducts(int categoryId, CancellationToken cancellationToken = default)
    {
        var spec = new SimilarProductByCategoryIdSpecification(categoryId);
        var similarProducts = await _productRepository.ListAsync(spec, cancellationToken);
        return similarProducts;
    }
}
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Product.Queries.Common;
using Demo_Delivery.Application.Specifications.Product;

namespace Demo_Delivery.Application.Product.Queries.GetAllAdminProducts;

public record GetAllProductsForAdminQuery(int? CategoryId): SortAndFilterBaseQuery<PaginatedList<AdminProductViewModel>>
{
    public const int PageSize = 10;
}

public class
    GetAllAdminProductsQueryHandler : IQueryHandler<GetAllProductsForAdminQuery,
    PaginatedList<AdminProductViewModel>>
{
    private readonly IReadRepository<Domain.Entities.ProductAggregate.Product> _productReadRepository;

    public GetAllAdminProductsQueryHandler(IReadRepository<Domain.Entities.ProductAggregate.Product> productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<PaginatedList<AdminProductViewModel>> Handle(GetAllProductsForAdminQuery request, CancellationToken cancellationToken)
    {
        var sortProducts = new SortProducts(request.sortBy, request.orderBy);

        var spec = new ProductsAsSummaryDtoSpecification(GetAllProductsForAdminQuery.PageSize,
            (request.currentPage - 1) * GetAllProductsForAdminQuery.PageSize, request.search, sortProducts, request.CategoryId);
        var products = await _productReadRepository.ListAsync(spec, cancellationToken);
        var count = await _productReadRepository.CountAsync(spec, cancellationToken);
        return new PaginatedList<AdminProductViewModel>(products, request.currentPage, GetAllProductsForAdminQuery.PageSize, count);
    }
}
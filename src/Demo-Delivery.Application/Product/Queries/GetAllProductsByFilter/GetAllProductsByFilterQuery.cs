using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Product.Queries.Common;
using Demo_Delivery.Application.Specifications.Product;

namespace Demo_Delivery.Application.Product.Queries.GetAllProductsByFilter;

using Product = Domain.Entities.ProductAggregate.Product;

public record GetAllProductsByFilterQuery(int? CategoryId) : SortAndFilterBaseQuery<PaginatedList<ProductViewModel>>
{
    public const int PageSize = 10;
}

public class GetAllByFilterHandler : IQueryHandler<GetAllProductsByFilterQuery, PaginatedList<ProductViewModel>>
{
    private readonly IReadRepository<Product> _repository;

    public GetAllByFilterHandler(IReadRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<ProductViewModel>> Handle(GetAllProductsByFilterQuery request,
        CancellationToken cancellationToken)
    {
        var sortProducts = new SortProducts(request.sortBy, request.orderBy);

        var spec = new ProductWithCategoriesAsDtoByFilterSpecification(take: GetAllProductsByFilterQuery.PageSize,
            skip: (request.currentPage - 1) * GetAllProductsByFilterQuery.PageSize, search: request.search,
            sort: sortProducts, categoryId: request.CategoryId);

        var productsDto = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(spec, cancellationToken);
        return new PaginatedList<ProductViewModel>(productsDto, request.currentPage,
            GetAllProductsByFilterQuery.PageSize, totalCount);
    }
}
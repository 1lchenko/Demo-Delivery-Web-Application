using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Category;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Specifications.Category;

namespace Demo_Delivery.Application.Category.Queries;

using Category = Domain.Entities.CategoryAggregate.Category;
public record GetAllCategoriesQuery : IQuery<List<CategoryViewModel>>;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryViewModel>>
{
    private readonly IReadRepository<Category> _categoryRepository;

    public GetAllCategoriesQueryHandler(IReadRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryViewModel>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAllCategoriesAsDtoSpecification();
        var categories = await _categoryRepository.ListAsync(spec, cancellationToken);
        return categories;
    }
}
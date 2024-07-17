using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Category;

namespace Demo_Delivery.Application.Specifications.Category;

using Category = Domain.Entities.CategoryAggregate.Category;

public class GetAllCategoriesAsDtoSpecification : Specification<Category, CategoryViewModel>
{
    public GetAllCategoriesAsDtoSpecification()
    {
        Query.Select(x => new CategoryViewModel
        {
            Id = x.Id.ToString(),
            Name = x.Name 
            
        }).AsNoTracking();
    }
}
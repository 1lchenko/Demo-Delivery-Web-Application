using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Category;

public class CategoryExistByIdSpecification : Specification<Domain.Entities.CategoryAggregate.Category, bool>
{
    public CategoryExistByIdSpecification(int categoryId)
    {
        Query.Where(x => x.Id == categoryId);
    }

   
}
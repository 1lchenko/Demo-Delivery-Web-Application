namespace Demo_Delivery.Domain.Filters;

public class CategoryFilter : BaseFilter
{
    public CategoryFilter(int currentPage, int pageSize, string? sortBy, string? search) : base(currentPage, pageSize, sortBy, search)
    {
    }
}
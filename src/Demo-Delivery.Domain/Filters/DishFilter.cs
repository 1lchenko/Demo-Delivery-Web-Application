namespace Demo_Delivery.Domain.Filters;

public class DishFilter : BaseFilter
{
    public DishFilter(int currentPage, int pageSize, string? sortBy, string? search, int? categoryId) : base(
        currentPage, pageSize, sortBy, search)
    {
        CategoryId = categoryId;
    }

    public int? CategoryId { get; set; }
}
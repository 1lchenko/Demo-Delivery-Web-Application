namespace Demo_Delivery.Domain.Filters;

public class BaseFilter
{
    protected BaseFilter(int currentPage, int pageSize, string? sortBy, string? search)
    {
        Skip = (currentPage - 1) * pageSize;
        Take = pageSize;
        SortBy = sortBy;
        Search = search;

        CurrentPage = currentPage;
        PageSize = pageSize;
    }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int Skip { get; set; } 
    public int Take { get; set; }  
    public string? SortBy { get; set; }
    public string? Search { get; set; }
}
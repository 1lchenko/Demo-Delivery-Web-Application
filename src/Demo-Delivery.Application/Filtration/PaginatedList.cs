using Microsoft.EntityFrameworkCore;

namespace Demo_Delivery.Application.Filtration;

public class PaginatedList<T> : List<T>
{
    public PaginatedList(List<T> items, int currentPage, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalCount = totalCount;
         
        AddRange(items);
    }

    

    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int TotalCount { get; private set; }

    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
    
    public static async Task<PaginatedList<T>> CreateFromQueryableAsync(IQueryable<T> source, int currentPage, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, currentPage, pageSize, count);
    }
}
using Demo_Delivery.Application.CQRS;

namespace Demo_Delivery.Application.Filtration;

public record SortAndFilterBaseQuery<T> : ISortAndFilterQuery<T>
{
    public int currentPage { get; init; } = 1;
    public string? search { get; init; }
    public string? orderBy { get; init; }
    public string? sortBy { get; init; }
    
}
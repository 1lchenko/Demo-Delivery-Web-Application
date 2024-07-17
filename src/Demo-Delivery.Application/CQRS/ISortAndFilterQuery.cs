using MediatR;

namespace Demo_Delivery.Application.CQRS;

public interface ISortAndFilterQuery<out T> : IQuery<T>
{
    public int currentPage { get; init; } 
    public string? search { get; init; }
    public string? orderBy { get; init; }
    public string? sortBy { get; init; }
}
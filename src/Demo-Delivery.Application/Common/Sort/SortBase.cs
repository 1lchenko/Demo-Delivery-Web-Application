using System.Linq.Expressions;

namespace Demo_Delivery.Application.Common.Sort;

public abstract class SortBase<TEntity>
{
    public const string Ascending = "asc";
    public const string Descending = "desc";

    protected SortBase(string? sortBy, string? orderBy)
    {
        this.SortBy = sortBy;
        this.OrderBy = orderBy;
    }

    public string? SortBy { get; }

    public string? OrderBy { get; }

    public abstract Expression<Func<TEntity, object>> ToExpression();
}
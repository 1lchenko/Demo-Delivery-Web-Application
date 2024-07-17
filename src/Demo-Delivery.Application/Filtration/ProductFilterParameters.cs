namespace Demo_Delivery.Application.Filtration;

public class ProductFilterParameters : PaginationParameters
{
    public string? SortBy { get; set; }
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
}
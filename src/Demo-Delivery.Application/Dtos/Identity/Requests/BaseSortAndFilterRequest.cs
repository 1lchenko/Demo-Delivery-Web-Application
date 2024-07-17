namespace Demo_Delivery.Application.Dtos.Account;

public class BaseSortAndFilterRequest
{
    public int CurrentPage { get; set; } = 1;
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public string? SortBy { get; set; }
}
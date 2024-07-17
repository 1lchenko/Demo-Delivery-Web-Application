using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo_Delivery.Api.Extensions;

public static class Pagination
{
    public static void AddPagination(this HttpResponse response, int currentPage, int totalCount,
        int totalPages, bool hasNextPage, bool hasPreviousPage)
    {
        var paginationHeader = new PaginationHeader(currentPage, totalPages, totalCount,hasNextPage, hasPreviousPage);
        var camelCaseFormatter = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}

public class PaginationHeader
{
    public PaginationHeader(int currentPage, int totalPages, int totalCount, bool hasNextPage, bool hasPreviousPage)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        TotalCount = totalCount;
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
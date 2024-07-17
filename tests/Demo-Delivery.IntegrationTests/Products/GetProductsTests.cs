using System.Net;
using System.Net.Http.Json;
using Demo_Delivery.Api;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Product.Queries.GetAllProductsByFilter;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.TestBase;
using Demo_Delivery.IntegrationTests.FakeRequests;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Demo_Delivery.IntegrationTests.Products;

public class GetProductsTest : DemoDeliveryIntegrationTestBase
{
    public GetProductsTest(TestFixture<Program, ApplicationDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }

    private async Task<HttpResponseMessage> GetAsync(GetAllProductsByFilterQuery query)
    {
        var apiUrl = ApiRoutes.Catalog.GetAllProducts;
        var queryParams = query.GetQueryString();
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get, RequestUri = new Uri($"{apiUrl}?{queryParams}", UriKind.RelativeOrAbsolute)
        };

        var client =   Fixture.CreateHttpClient(true);
        return await client.SendAsync(httpRequestMessage);
    }

    [Fact]
    public async Task Get_products_must_return_10_products_and_return_200()
    {
        var query = new FakeGetAllProductsByFilterQuery().Generate();
        var result = await GetAsync(query);
        var response = await result.Content.ReadFromJsonAsync<List<ProductViewModel>>();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Equal(GetAllProductsByFilterQuery.PageSize, response.Count);
        var paginationHeader = result.Headers.FirstOrDefault(x => x.Key == "Pagination");
        var paginationValues = JsonConvert.DeserializeObject<PaginatedOptions>(paginationHeader.Value.First());
        Assert.Equal(1, paginationValues.CurrentPage);
        Assert.True(paginationValues.TotalCount > 0);
        Assert.True(paginationValues.TotalPages > 0);
    }

    [Fact]
    public async Task Get_products_add_sort_and_order_by_filters_must_return_10_corresponding_products__and_return_200()
    {
        var query = new FakeGetAllProductsByFilterQuery(new Dictionary<string, object>()
        {
            { "sortBy", "price" }, { "orderBy", "desc" }
        }).Generate();

        var result = await GetAsync(query);
        var response = await result.Content.ReadFromJsonAsync<List<ProductViewModel>>();

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(response);
        Assert.NotEmpty(response);
        Assert.Equal(GetAllProductsByFilterQuery.PageSize, response.Count);
        Assert.True(response.SequenceEqual(response.OrderByDescending(p => p.Price)),
            "Products are not sorted by price in descending order");
        Assert.True(response.Count == GetAllProductsByFilterQuery.PageSize);

        var paginationHeader = result.Headers.FirstOrDefault(x => x.Key == "Pagination");
        var paginationValues = JsonConvert.DeserializeObject<PaginatedOptions>(paginationHeader.Value.First());
        Assert.Equal(1, paginationValues.CurrentPage);
        Assert.True(paginationValues.TotalCount > 0);
        Assert.True(paginationValues.TotalPages > 0);
    }
}
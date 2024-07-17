using System.Net.Http.Json;
using Demo_Delivery.Api;
using Demo_Delivery.Application.Dtos.Product;
using Demo_Delivery.Application.Product.Queries.GetProductById;
using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Domain.Entities.ProductAggregate;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.TestBase;
using Demo_Delivery.IntegrationTests.FakeRequests;
using Xunit.Abstractions;

namespace Demo_Delivery.IntegrationTests.Products;

public class GetProductByIdTests : DemoDeliveryIntegrationTestBase
{
    public GetProductByIdTests(TestFixture<Program, ApplicationDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }

    private async Task<HttpResponseMessage> GetAsync(GetProductByIdQuery query)
    {
        var apiUrl = ApiRoutes.Catalog.GetProductById + "/" + query.ProductId;

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get, RequestUri = new Uri($"{apiUrl}", UriKind.RelativeOrAbsolute)
        };

        var client =   Fixture.CreateHttpClient(true);
        return await client.SendAsync(httpRequestMessage);
    }

    [Fact]
    public async Task Get_product_by_id_have_to_return_200_Ok_and_valid_product()
    {
        var product = new Product("Name", "Description", 100, 100, 100, 100, true,
            new Category($"Category_{Guid.NewGuid().ToString()}"));

        product = await Fixture.InsertAsync(product);
        var query = new FakeGetProductByIdQuery(product.Id).Generate();
        var result = await GetAsync(query);
        var response = await result.Content.ReadFromJsonAsync<ProductDetailedViewModel>();
        Assert.NotNull(response);
        Assert.Equal(product.Id, response.Id);
        Assert.Equal(product.Name, response.Name);
        Assert.Equal(product.Description, response.Description);
        Assert.Equal(product.Category.Id, response.CategoryId);
        Assert.Equal(product.Category.Name, response.CategoryName);
        Assert.Equal(product.AmountOnStock, response.AmountOnStock);
        Assert.Equal(product.IsActive, response.IsActive);
    }
}
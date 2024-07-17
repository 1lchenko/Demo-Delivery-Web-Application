using System.Net;
using System.Net.Http.Headers;
using Demo_Delivery.Api;
using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.TestBase;
using Demo_Delivery.IntegrationTests.FakeRequests;


namespace Demo_Delivery.IntegrationTests.Products;

public class CreateProductTests : DemoDeliveryIntegrationTestBase
{
    public CreateProductTests(TestFixture<Program, ApplicationDbContext> integrationTestFixture) : base(
        integrationTestFixture)
    {
    }

    [Fact]
    public async Task Create_new_product_with_valid_data_have_to_return_code_200()
    {
        var category = new Category("TestCategory");
        await Fixture.InsertAsync(category);

        var command = new FakeCreateProductCommand(new Dictionary<string, object>() { { "CategoryId", category.Id } })
            .Generate();

        var formData = FormDataHelper.Create(command);

        var route = ApiRoutes.Catalog.CreateProduct;
        
        var client = Fixture.CreateHttpClient(true);

        
        var result = await client.PostAsync(route, formData);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

      
}
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Demo_Delivery.Api;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Order.Commands.CreateOrder;
using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.TestBase;
using Demo_Delivery.IntegrationTests.FakeRequests;
 

namespace Demo_Delivery.IntegrationTests.Orders;

public class CreateOrderTests : DemoDeliveryIntegrationTestBase
{
    public CreateOrderTests(TestFixture<Program, ApplicationDbContext> integrationTestFixture) : base(integrationTestFixture)
    {
    }

    [Fact]
    public async Task Create_new_order_with_valid_data_have_to_return_code_200()
    {
        var command = new FakeCreateOrderCommand().Generate();

        var route = ApiRoutes.Order.CreateOrder;
        var client = Fixture.CreateHttpClient(true);

        var result = await client.PostAsJsonAsync(route, command);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Create_new_order_with_inValid_voucher_have_to_return_code_404()
    {
        var command = new FakeCreateOrderCommand(new Dictionary<string, object>()
        {
            { nameof(CreateOrderCommand.VoucherCode), "FakeVoucher" }
        }).Generate();

        var route = ApiRoutes.Order.CreateOrder;
        var exceptionMessage = new NotFoundException(nameof(Voucher), command.VoucherCode).Message;

        var client = Fixture.CreateHttpClient(true);
        var result = await client.PostAsync(route, JsonContent.Create(command));

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

        var content = await result.Content.ReadAsStringAsync();

        Assert.Contains(exceptionMessage, content);
    }
}
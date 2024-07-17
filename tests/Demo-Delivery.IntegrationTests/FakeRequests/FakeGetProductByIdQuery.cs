using AutoBogus;
using Demo_Delivery.Application.Product.Queries.GetAllProductsByFilter;
using Demo_Delivery.Application.Product.Queries.GetProductById;

namespace Demo_Delivery.IntegrationTests.FakeRequests;

public sealed class FakeGetProductByIdQuery : AutoFaker<GetProductByIdQuery>
{
    public FakeGetProductByIdQuery(int? productId = null)
    {
        RuleFor(q => q.ProductId, _ => productId != null ? productId.Value : 1);
    }
}
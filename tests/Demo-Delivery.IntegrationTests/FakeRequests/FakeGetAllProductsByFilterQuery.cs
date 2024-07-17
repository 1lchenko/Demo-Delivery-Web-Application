using AutoBogus;
using Demo_Delivery.Application.Product.Queries.GetAllProductsByFilter;

namespace Demo_Delivery.IntegrationTests.FakeRequests;

public sealed class FakeGetAllProductsByFilterQuery : AutoFaker<GetAllProductsByFilterQuery>
{
    public FakeGetAllProductsByFilterQuery(Dictionary<string, object> args = null)
    {
        RuleFor(q => q.CategoryId, _ => args != null && args.ContainsKey("CategoryId") ? (int?)args["CategoryId"] : null);
        RuleFor(q => q.currentPage, _ => args != null && args.ContainsKey("currentPage") ? (int)args["currentPage"] : 1);
        RuleFor(q => q.search, _ => args != null && args.ContainsKey("search") ? (string)args["search"] : null);
        RuleFor(q => q.orderBy, _ => args != null && args.ContainsKey("orderBy") ? (string)args["orderBy"] : null);
        RuleFor(q => q.sortBy, _ => args != null && args.ContainsKey("sortBy") ? (string)args["sortBy"] : null);
    }
}

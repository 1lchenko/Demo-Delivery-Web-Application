using AutoBogus;
using Demo_Delivery.Application.Product.Commands.CreateProduct;
using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.IntegrationTests.FakeRequests;

public sealed class FakeCreateProductCommand : AutoFaker<CreateProductCommand>
{
    public FakeCreateProductCommand(Dictionary<string, object> args = null)
    {
        RuleFor(p => p.Name, _ => args != null && args.ContainsKey("Name") ? (string)args["Name"] : "ProductName");
        RuleFor(p => p.Description, _ => args != null && args.ContainsKey("Description") ? (string)args["Description"] : "Product Description Product Description Product Description");
        RuleFor(p => p.CategoryId, _ => args != null && args.ContainsKey("CategoryId") ? (int)args["CategoryId"] : 1);
        RuleFor(p => p.ImageFiles, _ => args != null && args.ContainsKey("ImageFiles") ? (List<IFormFile>)args["ImageFiles"] : new List<IFormFile>());
        RuleFor(p => p.Calories, _ => args != null && args.ContainsKey("Calories") ? (int)args["Calories"] : 100);
        RuleFor(p => p.Price, _ => args != null && args.ContainsKey("Price") ? (decimal)args["Price"] : 100.0m);
        RuleFor(p => p.IsActive, _ => args != null && args.ContainsKey("IsActive") ? (bool)args["IsActive"] : true);
        RuleFor(p => p.AmountOnStock, _ => args != null && args.ContainsKey("AmountOnStock") ? (int)args["AmountOnStock"] : 100);
        RuleFor(p => p.MaxStockThreshold, _ => args != null && args.ContainsKey("MaxStockThreshold") ? (int)args["MaxStockThreshold"] : 100);
    }

}
using Ardalis.Specification;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Product.Commands.CreateProduct;
using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Microsoft.AspNetCore.Http;
 

namespace Demo_Delivery.UnitTests.Application.Product;

public class CreateProductCommandTest
{
    private readonly IRepository<Demo_Delivery.Domain.Entities.ProductAggregate.Product> _productRepositoryMock;
    private readonly IReadRepository<Category> _categoryRepositoryMock;
    private readonly IFileService _fileServiceMock;

    public CreateProductCommandTest()
    {
        _productRepositoryMock = Substitute.For<IRepository<Demo_Delivery.Domain.Entities.ProductAggregate.Product>>();
        _categoryRepositoryMock = Substitute.For<IReadRepository<Category>>();
        _fileServiceMock = Substitute.For<IFileService>();
    }


    [Fact]
    public async Task Handle_throws_exception_when_category_is_null()
    {
        var product = new ProductBuilder().Build();
        var cmd = GetFakeCreateProductCommand();
        _categoryRepositoryMock.GetByIdAsync(Arg.Any<ISpecification<Category>>(), Arg.Any<CancellationToken>())
            .Returns((Category)null);
        
        //Act
        var handler = new CreateProductHandler(_productRepositoryMock, _fileServiceMock, _categoryRepositoryMock);
        var cltToken = new CancellationToken();
        async Task Act() => await handler.Handle(cmd, cltToken);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(Act);
    }
    
    
    private CreateProductCommand GetFakeCreateProductCommand(Dictionary<string, object> args = null)
    {
        return new CreateProductCommand(Name: args != null && args.ContainsKey("Name") ? (string)args["Name"] : null,
            Description: args != null && args.ContainsKey("Description") ? (string)args["Description"] : null,
            Price: args != null && args.ContainsKey("Price") ? (decimal)args["Price"] : 1,
            CategoryId: args != null && args.ContainsKey("CategoryId") ? (int)args["CategoryId"] : 1,
            Calories: args != null && args.ContainsKey("Calories") ? (int)args["Calories"] : 1,
            AmountOnStock: args != null && args.ContainsKey("AmountOnStock") ? (int)args["AmountOnStock"] : 1,
            MaxStockThreshold: args != null && args.ContainsKey("MaxStockThreshold") ? (int)args["MaxStockThreshold"] : 1,
            IsActive: args != null && args.ContainsKey("IsActive") ? (bool)args["IsActive"] : true,
            ImageFiles: args != null && args.ContainsKey("ImageFiles") ? (List<IFormFile>)args["ImageFiles"] : null);
    }
}
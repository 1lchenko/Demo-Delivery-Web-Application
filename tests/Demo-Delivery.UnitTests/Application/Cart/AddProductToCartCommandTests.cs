using Ardalis.Specification;
using AutoMapper;
using Demo_Delivery.Application.Cart.Commands.AddProductToCart;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Domain.Entities.CustomerAggregate;
 

namespace Demo_Delivery.UnitTests.Application.Cart;

public class AddProductToCartCommandTests
{
    private readonly IRepository<Demo_Delivery.Domain.Entities.CartAggregate.Cart> _cartRepositoryMock;
    private readonly IReadRepository<Demo_Delivery.Domain.Entities.ProductAggregate.Product> _productReadRepositoryMock;
    private readonly IReadRepository<Customer> _customerRepositoryMock;
    private readonly IUser _currentUserMock;
    private readonly IMapper _mapper;

    public AddProductToCartCommandTests()
    {
        _cartRepositoryMock = Substitute.For<IRepository<Demo_Delivery.Domain.Entities.CartAggregate.Cart>>();
        _currentUserMock = Substitute.For<IUser>();
        _productReadRepositoryMock =
            Substitute.For<IReadRepository<Demo_Delivery.Domain.Entities.ProductAggregate.Product>>();
        _customerRepositoryMock = Substitute.For<IReadRepository<Customer>>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async Task Handle_throws_exception_when_product_is_not_found()
    {
        var cmd = GetFakeAddProductToCartCommand();
        _productReadRepositoryMock
            .GetByIdAsync(Arg.Any<ISpecification<Demo_Delivery.Domain.Entities.ProductAggregate.Product>>(),
                Arg.Any<CancellationToken>())
            .Returns((Demo_Delivery.Domain.Entities.ProductAggregate.Product)null);
        var handler = new AddItemToCartCommandHandler(_cartRepositoryMock, _productReadRepositoryMock, _currentUserMock,
            _customerRepositoryMock, _mapper);

        //Act 
        Task<CartViewModel> Act()
        {
            return handler.Handle(cmd, new CancellationToken());
        }

        //Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(Act);
    }

    private AddProductToCartCommand GetFakeAddProductToCartCommand(Dictionary<string, object> args = null)
    {
        return new AddProductToCartCommand(args != null && args.ContainsKey("ProductId") ? (int)args["ProductId"] : 1);
    }
}
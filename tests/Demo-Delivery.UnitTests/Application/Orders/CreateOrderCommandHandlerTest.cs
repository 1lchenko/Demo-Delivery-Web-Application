using Ardalis.Specification;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Order.Commands.CreateOrder;
using Demo_Delivery.Domain.Entities.CartAggregate;
using Demo_Delivery.Domain.Entities.CustomerAggregate;
using Demo_Delivery.Domain.Entities.OrderAggregate;
using Demo_Delivery.Domain.Entities.VoucherAggregate;
 

namespace Demo_Delivery.UnitTests.Application.Orders;

public class CreateOrderCommandHandlerTest
{
    private readonly IReadRepository<CartItem> _cartItemRepositoryMock;
    private readonly IReadRepository<Voucher> _voucherReadRepositoryMock;
    private readonly IReadRepository<Customer> _customerRepositoryMock;
    private readonly IRepository<Order> _orderRepositoryMock;
    private readonly IUser _currentUserMock;

    public CreateOrderCommandHandlerTest()
    {
        _orderRepositoryMock = Substitute.For<IRepository<Order>>();
        _currentUserMock = Substitute.For<IUser>();
        _voucherReadRepositoryMock = Substitute.For<IReadRepository<Voucher>>();
        _customerRepositoryMock = Substitute.For<IReadRepository<Customer>>();
        _cartItemRepositoryMock = Substitute.For<IReadRepository<CartItem>>();
    }

    [Fact]
    public async Task Handle_throws_exception_when_customer_is_null()
    {
        var nonexistUserId = "0";
        var fakeCreateOrderCmd = GetFakeCreateOrderCommand();
        _currentUserMock.Id.Returns(nonexistUserId);

        //Act
        var handler = new CreateOrderCommandHandler(_cartItemRepositoryMock, _voucherReadRepositoryMock,
            _orderRepositoryMock, _currentUserMock, _customerRepositoryMock);
        var cltToken = new CancellationToken();
        async Task<int> Act() => await handler.Handle(fakeCreateOrderCmd, cltToken);

        //Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(Act);
        Assert.Equal("Order cannot be created. Customer does not exist.", exception.Message);
    }

    [Fact]
    public async Task Handle_throws_exception_when_cartItems_nonExist()
    {
        var fakeCreateOrderCmd = GetFakeCreateOrderCommand();
        _cartItemRepositoryMock.ListAsync(Arg.Any<ISpecification<CartItem>>()).Returns([]);
        var fakeCustomer = GetFakeCustomer();
        _customerRepositoryMock.FirstOrDefaultAsync(Arg.Any<ISpecification<Customer>>()).Returns(fakeCustomer);
        var existUserId = "123";
        _currentUserMock.Id.Returns(existUserId);

        //Act
        var handler = new CreateOrderCommandHandler(_cartItemRepositoryMock, _voucherReadRepositoryMock,
            _orderRepositoryMock, _currentUserMock, _customerRepositoryMock);
        var cltToken = new CancellationToken();
        async Task<int> Act() => await handler.Handle(fakeCreateOrderCmd, cltToken);

        //Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(Act);
        Assert.Equal($"Customer {fakeCustomer.Id} does not have an existing shopping cart.", exception.Message);
    }

    private Customer GetFakeCustomer()
    {
        return new Customer("Test Name", "Test UserId", "TestEmail@gmail.com");
    }

    private CreateOrderCommand GetFakeCreateOrderCommand(Dictionary<string, object> args = null)
    {
        return new CreateOrderCommand(
            VoucherCode: args != null && args.ContainsKey("VoucherCode") ? (string)args["VoucherCode"] : null,
            Comment: args != null && args.ContainsKey("Comment") ? (string)args["Comment"] : null,
            WhenDeliver: args != null && args.ContainsKey("WhenDeliver") ? (DateTime)args["WhenDeliver"] : null,
            DeliverForNearFuture: args != null && args.ContainsKey("WhenDeliver") ? (bool)args["WhenDeliver"] : false,
            Address: new AddressDto
            {
                ApartmentNumber =
                    args != null && args.ContainsKey("ApartmentNumber") ? (int)args["ApartmentNumber"] : 0,
                BuildingNumber =
                    args != null && args.ContainsKey("BuildingNumber") ? (string)args["BuildingNumber"] : null,
                IntercomPinCode =
                    args != null && args.ContainsKey("IntercomPinCode") ? (string)args["IntercomPinCode"] : null,
                Note = args != null && args.ContainsKey("Note") ? (string)args["Note"] : null,
                Street = args != null && args.ContainsKey("Street") ? (string)args["Street"] : null,
            });
    }
}
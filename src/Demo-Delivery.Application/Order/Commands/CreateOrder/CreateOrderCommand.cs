using Ardalis.Specification;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Application.Specifications.Voucher;
using Demo_Delivery.Domain.Entities.CartAggregate;
using Demo_Delivery.Domain.Entities.CustomerAggregate;
using Demo_Delivery.Domain.Entities.OrderAggregate;
using Demo_Delivery.Domain.Entities.VoucherAggregate;

namespace Demo_Delivery.Application.Order.Commands.CreateOrder;

using Address = Address;
using Order = Domain.Entities.OrderAggregate.Order;

public record CreateOrderCommand(
    string? VoucherCode,
    string Comment,
    DateTime? WhenDeliver,
    bool DeliverForNearFuture,
    AddressDto Address) : ICommand<int>
{
    
}

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, int>
{
    private readonly IReadRepository<CartItem> _cartItemRepository;
    private readonly IReadRepository<Voucher> _voucherReadRepository;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IUser _currentUser;

    public CreateOrderCommandHandler(IReadRepository<CartItem> cartItemRepository,
        IReadRepository<Voucher> voucherReadRepository, IRepository<Order> orderRepository, IUser currentUser,
        IReadRepository<Customer> customerRepository)
    {
        _cartItemRepository = cartItemRepository;
        _voucherReadRepository = voucherReadRepository;
        _orderRepository = orderRepository;
        _currentUser = currentUser;
        _customerRepository = customerRepository;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException("Order cannot be created. Customer does not exist.");
        }

        var spec = new CartItemsByCustomerIdSpecification(customer.Id);
        var cartItems = await GetCartItemsAsync(spec, customer.Id, cancellationToken);

        var address = new Address(request.Address.Street, request.Address.IntercomPinCode,
            request.Address.BuildingNumber, request.Address.ApartmentNumber, request.Address.Note);
        var order = new Order(customer.Id, request.Comment, request.WhenDeliver, request.DeliverForNearFuture, address);

        foreach (var cartItem in cartItems)
        {
            order.AddOrderItem(cartItem.ProductId, cartItem.ProductName, cartItem.UnitPrice, cartItem.ImageKey,
                cartItem.Quantity);
        }

        await ApplyVoucher(order, request.VoucherCode, cancellationToken);

        await _orderRepository.AddAsync(order, cancellationToken);

        return order.Id;
    }

    private async Task<IEnumerable<CartItem>> GetCartItemsAsync(ISpecification<CartItem> spec, int customerId,
        CancellationToken cancellationToken)
    {
        var cartItems = await _cartItemRepository.ListAsync(spec, cancellationToken);
        if (!cartItems.Any())
        {
            throw new BadRequestException($"Customer {customerId} does not have an existing shopping cart.");
        }

        return cartItems;
    }

    private async Task ApplyVoucher(Order order, string voucherCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(voucherCode))
        {
            var spec = new VoucherByCodeSpecification(voucherCode);
            var voucher = await _voucherReadRepository.FirstOrDefaultAsync(spec, cancellationToken);
            if (voucher is null)
            {
                throw new NotFoundException(nameof(Voucher), voucherCode);
            }

            order.SetVoucher(voucher);
        }
    }
}
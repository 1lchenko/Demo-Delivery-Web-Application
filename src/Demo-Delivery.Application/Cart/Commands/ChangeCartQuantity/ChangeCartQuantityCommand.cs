using AutoMapper;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Cart;
using Demo_Delivery.Application.Specifications.Cart;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Cart.Commands.ChangeCartQuantity;

using Cart = Domain.Entities.CartAggregate.Cart;
using Product = Domain.Entities.ProductAggregate.Product;

public record ChangeCartQuantityCommand(int ProductId, int Quantity) : ICommand<CartViewModel>;

public class ChangeCartQuantityCommandHandler : ICommandHandler<ChangeCartQuantityCommand, CartViewModel>
{
    private readonly IRepository<Cart> _cartRepository;
    private readonly IReadRepository<Product> _productRepository;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUser _currentUser;

    public ChangeCartQuantityCommandHandler(IRepository<Cart> cartRepository, IUser currentUser,
        IReadRepository<Product> productRepository, IReadRepository<Customer> customerRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _currentUser = currentUser;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CartViewModel> Handle(ChangeCartQuantityCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);
        }

        var customerSpec = new CustomerByUserIdSpecification(_currentUser.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(customerSpec, cancellationToken);

        if (customer is null)
        {
            throw new BadRequestException($"User '{_currentUser.Id}' does not have an associated customer.");
        }

        var cartSpec = new CartByCustomerIdSpecification(customer.Id);
        var cart = await _cartRepository.FirstOrDefaultAsync(cartSpec, cancellationToken);

        if (cart is null)
        {
            throw new BadRequestException($"Customer '{customer.Id}' does not have an existing shopping cart.");
        }

        cart.UpdateCartItemQuantity(product.Id, request.Quantity);

        await _cartRepository.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartViewModel>(cart);
        return cartDto;
    }
}
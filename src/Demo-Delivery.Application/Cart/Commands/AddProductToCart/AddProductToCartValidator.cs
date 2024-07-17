using FluentValidation;

namespace Demo_Delivery.Application.Cart.Commands.AddProductToCart;

public class AddProductToCartValidator : AbstractValidator<AddProductToCartCommand>
{
    public AddProductToCartValidator()
    {
        RuleFor(cmd => cmd.ProductId).NotEmpty().GreaterThan(0);
    }
}
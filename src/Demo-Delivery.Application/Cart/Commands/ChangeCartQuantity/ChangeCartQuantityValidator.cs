using FluentValidation;

namespace Demo_Delivery.Application.Cart.Commands.ChangeCartQuantity;

public class ChangeCartQuantityValidator : AbstractValidator<ChangeCartQuantityCommand>
{
    public ChangeCartQuantityValidator()
    {
        RuleFor(cmd => cmd.ProductId).NotEmpty().GreaterThan(0);
        RuleFor(cmd => cmd.Quantity).GreaterThanOrEqualTo(0);
    }
}
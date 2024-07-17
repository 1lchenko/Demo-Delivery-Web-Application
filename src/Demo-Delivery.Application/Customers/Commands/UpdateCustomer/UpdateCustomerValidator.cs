using Demo_Delivery.Domain;
using FluentValidation;

namespace Demo_Delivery.Application.Customers.Commands.UpdateCustomer;
using static DomainConstants.CategoryValidationConstants;
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerValidator()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .MinimumLength(MinNameLength)
            .MaximumLength(MaxNameLength);
        RuleFor(cmd => cmd.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(cmd => cmd.AdminComment)
            .MaximumLength(MaxCommentLength);
    }
}
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Domain;
using FluentValidation;

namespace Demo_Delivery.Application.Order.Commands.CreateOrder;

using static DomainConstants.OrderValidationConstants;
using static DomainConstants.AddressValidationConstants;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(cmd => cmd.VoucherCode).MaximumLength(MaxVoucherCodeLength);

        RuleFor(cmd => cmd.Comment).MaximumLength(MaxCommentLength);

        RuleFor(cmd => cmd.WhenDeliver)
            .Must(date => !date.HasValue || date.Value > DateTime.Now)
            .WithMessage("{PropertyName} must be in the future");

        RuleFor(cmd => cmd.DeliverForNearFuture).NotNull().WithMessage("{PropertyName} must be specified");

        RuleFor(cmd => cmd.Address).SetValidator(new AddressDtoValidator());
    }
}

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(addr => addr.Street).NotEmpty().MinimumLength(MinStreetLength).MaximumLength(MaxStreetLength);

        RuleFor(addr => addr.IntercomPinCode).Length(MinIntercomPinCodeLength, MaxIntercomPinCodeLength);

        RuleFor(addr => addr.BuildingNumber)
            .NotEmpty()
            .MinimumLength(MinBuildingNumberLength)
            .MaximumLength(MaxBuildingNumberLength);

        RuleFor(addr => addr.ApartmentNumber)
            .NotEmpty()
            .GreaterThanOrEqualTo(MinApartmentNumber)
            .LessThanOrEqualTo(MaxApartmentNumber);

        RuleFor(addr => addr.Note).MinimumLength(MinNoteLength).MaximumLength(MaxNoteLength);
    }
}
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Domain;
using FluentValidation;

namespace Demo_Delivery.Application.Order.Commands.UpdateOrder;

using static DomainConstants.AddressValidationConstants;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(cmd => cmd.Id).GreaterThan(0);

        RuleFor(cmd => cmd.WhenDeliver)
            .Must(date => !date.HasValue || date.Value > DateTime.Now)
            .WithMessage("{PropertyName} must be set for the future");

        RuleFor(cmd => cmd).Must(DataSetProperly);

        RuleFor(cmd => cmd.Address).SetValidator(new AddressDtoValidator());
    }

    private bool DataSetProperly(UpdateOrderCommand cmd)
    {
        return cmd is not { WhenDeliver: null, DeliverForNearFuture: false } &&
               cmd is not { WhenDeliver: not null, DeliverForNearFuture: true };
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
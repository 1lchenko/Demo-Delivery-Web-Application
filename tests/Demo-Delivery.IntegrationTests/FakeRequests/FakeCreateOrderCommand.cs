using AutoBogus;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Application.Order.Commands.CreateOrder;
using Demo_Delivery.Domain.Entities.VoucherAggregate;

namespace Demo_Delivery.IntegrationTests.FakeRequests;

public sealed class FakeCreateOrderCommand : AutoFaker<CreateOrderCommand>
{
    public FakeCreateOrderCommand(Dictionary<string, object> args = null)
    {
        RuleFor(p => p.VoucherCode, _ => args != null && args.ContainsKey("VoucherCode") ? (string)args["VoucherCode"] : null);
        RuleFor(p => p.Comment, _ => args != null && args.ContainsKey("Comment") ? (string)args["Comment"] : "Comment");
        RuleFor(p => p.WhenDeliver, _ => args != null && args.ContainsKey("WhenDeliver") ? (DateTime)args["WhenDeliver"] : DateTime.Now.Add(TimeSpan.FromHours(3)));
        RuleFor(p => p.DeliverForNearFuture, _ => args != null && args.ContainsKey("DeliverForNearFuture") ? (bool)args["DeliverForNearFuture"] : false);
        RuleFor(p => p.Address,
            _ => new AddressDto()
            {
                Street = args != null && args.ContainsKey("Street") ? (string)args["Street"] : "Street",
                ApartmentNumber = args != null && args.ContainsKey("ApartmentNumber") ? (int)args["ApartmentNumber"] : 10,
                BuildingNumber = args != null && args.ContainsKey("BuildingNumber") ? (string)args["BuildingNumber"] : "10",
                IntercomPinCode = args != null && args.ContainsKey("IntercomPinCode") ? (string)args["IntercomPinCode"] : "123",
                Note = args != null && args.ContainsKey("Note") ? (string)args["Note"] : "Note"
            });
    }

}
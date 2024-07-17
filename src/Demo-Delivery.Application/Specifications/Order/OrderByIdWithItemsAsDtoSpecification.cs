using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Order;
using Demo_Delivery.Domain;

namespace Demo_Delivery.Application.Specifications.Order;

public class
    OrderByIdWithItemsAsDtoSpecification : Specification<Domain.Entities.OrderAggregate.Order, OrderDetailedViewModel>
{
    public OrderByIdWithItemsAsDtoSpecification(int orderId, bool userIsAdmin)
    {
        Query.Select(x => new OrderDetailedViewModel
            {
                Id = x.Id.ToString(),
                CreateOrderOn = x.CreatedDate,
                Status = x.OrderStatus.ToString(),
                FinalPrice = x.FinalPrice,
                BasePrice = x.BasePrice,
                Discount = x.Discount,
                Comment = x.Comment,
                AddressDto =
                    new AddressDto
                    {
                        Street = x.Address.Street,
                        IntercomPinCode = x.Address.IntercomPinCode,
                        BuildingNumber = x.Address.BuildingNumber,
                        ApartmentNumber = x.Address.ApartmentNumber,
                        Note = x.Address.Note
                    },
                WhenDeliver = x.WhenDeliver,
                OrderItems = x.OrderItems.Select(t => new OrderItemDetailedDto
                    {
                        ImageKey = t.ImageKey,
                        ProductName = t.ProductName,
                        Quantity = t.Quantity,
                        TotalPrice = t.TotalPrice
                    })
                    .ToList()
            })
            .Where(x => x.Id == orderId && (userIsAdmin || x.Id == orderId));
    }
}
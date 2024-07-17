using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Order;

namespace Demo_Delivery.Application.Specifications.Order;

public class
    CustomerOrderDetailedByIdSpecification : Specification<Domain.Entities.OrderAggregate.Order,
    CustomerOrderDetailedViewModel>
{
    public CustomerOrderDetailedByIdSpecification(int orderId)
    {
        Query.Select(x => new CustomerOrderDetailedViewModel
            {
                Id = x.Id,
                CreateOn = x.CreatedDate,
                Status = x.OrderStatus.ToString(),
                Discount = x.Discount,
                Comment = x.Comment,
                CustomerId = x.CustomerId,
                Address =
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
            .Where(x => x.Id == orderId)
            .AsNoTracking();
    }
}
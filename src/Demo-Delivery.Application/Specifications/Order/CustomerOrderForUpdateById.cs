using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Order;

namespace Demo_Delivery.Application.Specifications.Order;

public class CustomerOrderForUpdateById : Specification<Domain.Entities.OrderAggregate.Order, CustomerOrderForUpdateDto>
{
    public CustomerOrderForUpdateById(int id)
    {
        Query

            .Select(x => new CustomerOrderForUpdateDto
            {
                Id = x.Id,
                Status = x.OrderStatus.Name,
                WhenDeliver = x.WhenDeliver,
                DeliverForNearFuture = x.DeliverForNearFuture,
                Comment = x.Comment,
                Address = new AddressDto
                {
                    Street = x.Address.Street,
                    BuildingNumber = x.Address.BuildingNumber,
                    ApartmentNumber = x.Address.ApartmentNumber,
                    IntercomPinCode = x.Address.IntercomPinCode,
                    Note = x.Address.Note
                }
            })
            .Where(x => x.Id == id);
    }
}
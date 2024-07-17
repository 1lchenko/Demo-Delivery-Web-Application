using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Customer;
using Demo_Delivery.Application.Dtos.Order;

namespace Demo_Delivery.Application.Specifications.Customer;

public class CustomerAsDtoByIdSpecification : Specification<Domain.Entities.CustomerAggregate.Customer, CustomerViewModel>
{
    public CustomerAsDtoByIdSpecification(int id)
    {
        Query.Select(x => new CustomerViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                LastPurchaseDate = x.LastPurchaseDateUtc,
                LastUpdateCartDate = x.LastUpdateCartDateUtc,
                AdminComment = x.AdminComment
            })
            .Where(x => x.Id == id);
    }   
}
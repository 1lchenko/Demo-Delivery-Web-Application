using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Customer;

namespace Demo_Delivery.Application.Specifications.Customer;

public class CustomerDetailsAsDtoByIdSpecification : Specification<Domain.Entities.CustomerAggregate.Customer, CustomerDetailedViewModel>
{
    public CustomerDetailsAsDtoByIdSpecification(int id)
    {
        Query.Select(x => new CustomerDetailedViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            LastPurchaseDate = x.LastPurchaseDateUtc,
            LastUpdateCartDate = x.LastUpdateCartDateUtc,
            AdminComment = x.AdminComment,
        })
        .Where(x => x.Id == id);
    }
}
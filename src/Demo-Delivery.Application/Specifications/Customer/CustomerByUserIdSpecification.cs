using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Customer;

public class CustomerByUserIdSpecification : Specification<Domain.Entities.CustomerAggregate.Customer>
{
    public CustomerByUserIdSpecification(string userId)
    {
        Query.Where(x => x.UserId == userId);
        
      
    }
}
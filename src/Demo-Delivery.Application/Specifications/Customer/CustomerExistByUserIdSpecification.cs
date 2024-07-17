using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Customer;

public class CustomerExistByUserIdSpecification : Specification<Domain.Entities.CustomerAggregate.Customer>
{
    public CustomerExistByUserIdSpecification(int id)
    {
        Query.Where(x => x.Id == id);
    }

    
}
using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Customer;

public class CustomersByIdSpecification : Specification<Domain.Entities.CustomerAggregate.Customer>
{
    public CustomersByIdSpecification(List<int> customerIds)
    {
        Query.Where(x => customerIds.Contains(x.Id));
    }
}
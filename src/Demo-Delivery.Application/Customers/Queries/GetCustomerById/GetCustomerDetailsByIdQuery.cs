using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Dtos.Customer;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(int Id) : IQuery<CustomerViewModel>;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerViewModel>
{
    private readonly IReadRepository<Customer> _customerRepository;

    public GetCustomerByIdQueryHandler(IReadRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
      
    }

    public async Task<CustomerViewModel> Handle(GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new CustomerAsDtoByIdSpecification(request.Id);
        var customer = await _customerRepository.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (customer is null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

         
        return customer;
    }
}
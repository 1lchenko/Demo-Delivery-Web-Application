using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    int Id,
    string Name,
    string Email,
    string AdminComment) : ICommand;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IRepository<Customer> _customerRepository;

    public UpdateCustomerCommandHandler(IRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

        customer.Update(request.Name , request.Email, request.AdminComment);
        
        await _customerRepository.UpdateAsync(customer, cancellationToken);
    }
}
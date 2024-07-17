using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.CQRS;
using Demo_Delivery.Application.Customers.Common;
using Demo_Delivery.Application.Dtos.Customer;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Application.Specifications.Customer;
using Demo_Delivery.Domain.Entities.CustomerAggregate;

namespace Demo_Delivery.Application.Customers.Queries.GetAllCustomers;

public record GetAllCustomersQuery(
    DateTime? LastPurchaseDateFrom,
    DateTime? LastPurchaseDateTo,
    DateTime? LastCartUpdateDateFrom,
    DateTime? LastCartUpdateDateTo) : SortAndFilterBaseQuery<PaginatedList<CustomerViewModel>>
{
    public const int PageSize = 10;
}

public class GetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, PaginatedList<CustomerViewModel>>
{
    private readonly IReadRepository<Customer> _customerRepository;

    public GetAllCustomersQueryHandler(IReadRepository<Customer> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PaginatedList<CustomerViewModel>> Handle(GetAllCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var sortOption = new SortCustomers(request.sortBy, request.orderBy);
        
        var spec = new AllCustomersAsDtoSpecification(request.search, request.LastPurchaseDateFrom,
            request.LastPurchaseDateTo, request.LastCartUpdateDateFrom, request.LastCartUpdateDateTo, sortOption,
            GetAllCustomersQuery.PageSize, (request.currentPage - 1) * GetAllCustomersQuery.PageSize);
        
        var customers = await _customerRepository.ListAsync(spec, cancellationToken);
        
        var count = await _customerRepository.CountAsync(spec, cancellationToken);
        
        return new PaginatedList<CustomerViewModel>(customers, request.currentPage, GetAllCustomersQuery.PageSize,
            count);
    }
}
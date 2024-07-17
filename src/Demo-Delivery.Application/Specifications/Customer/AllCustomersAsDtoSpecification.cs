using Ardalis.Specification;
using Demo_Delivery.Application.Common.Sort;
using Demo_Delivery.Application.Dtos.Customer;
using Demo_Delivery.Application.Dtos.Order;

namespace Demo_Delivery.Application.Specifications.Customer;

public class
    AllCustomersAsDtoSpecification : Specification<Domain.Entities.CustomerAggregate.Customer, CustomerViewModel>
{
    public AllCustomersAsDtoSpecification(string? search, DateTime? lastPurchaseDateFrom, DateTime? lastPurchaseDateTo,
        DateTime? lastCartUpdateDateFrom, DateTime? lastCartUpdateDateTo,
        SortBase<Domain.Entities.CustomerAggregate.Customer> sort, int take, int skip)
    {
        Query
            .Select(x => new CustomerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                LastPurchaseDate = x.LastPurchaseDateUtc,
                LastUpdateCartDate = x.LastUpdateCartDateUtc,
                AdminComment = x.AdminComment
            })
            .Where(x => x.LastPurchaseDateUtc >= lastPurchaseDateFrom!.Value, lastPurchaseDateFrom.HasValue)
            .Where(x => x.LastUpdateCartDateUtc <= lastPurchaseDateTo!.Value, lastPurchaseDateTo.HasValue)
            .Where(x => x.LastPurchaseDateUtc >= lastCartUpdateDateFrom!.Value, lastCartUpdateDateFrom.HasValue)
            .Where(x => x.LastUpdateCartDateUtc <= lastCartUpdateDateTo!.Value, lastCartUpdateDateTo.HasValue)
            .Where(
                x =>  x.Email.Contains(search!) || x.Name.Contains(search!) ||
                     x.AdminComment.Contains(search!), !string.IsNullOrWhiteSpace(search))
            .ApplyOrdering(sort)
            .Skip(skip)
            .Take(take);
    }
}
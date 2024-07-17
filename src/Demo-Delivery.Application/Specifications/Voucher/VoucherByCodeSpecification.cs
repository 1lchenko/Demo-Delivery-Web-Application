using Ardalis.Specification;

namespace Demo_Delivery.Application.Specifications.Voucher;

public class VoucherByCodeSpecification : Specification<Domain.Entities.VoucherAggregate.Voucher>
{
    public VoucherByCodeSpecification(string code)
    {
        Query.Include(x => x.VoucherDiscountType)
            .Where(x => x.Code == code);
    }
}
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.VoucherAggregate;

public class VoucherDiscountType : Enumeration
{
    public static VoucherDiscountType Percentage = new VoucherDiscountType(1, nameof(Percentage).ToLowerInvariant());
    public static VoucherDiscountType Value = new VoucherDiscountType(2, nameof(Value).ToLowerInvariant());

    public VoucherDiscountType(int id, string name) : base(id, name)
    {
    }

    public static IEnumerable<VoucherDiscountType> List() => new[] { Percentage, Value };

    public static VoucherDiscountType FromName(string name)
    {
        var status = List().FirstOrDefault(x => String.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (status == null)
        {
            throw new VoucheringDomainException(ValuesToString());
        }

        return status;
    }

    public static VoucherDiscountType FromId(int id)
    {
        var status = List().FirstOrDefault(x => x.Id == id);
        if (status == null)
        {
            throw new VoucheringDomainException(ValuesToString());
        }

        return status;
    }

    public static string ValuesToString() =>
        $"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}";
}
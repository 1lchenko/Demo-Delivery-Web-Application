using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.EntityData;

public class VoucherDiscountTypeData : IInicialData
{
    public Type EntityType => typeof(VoucherDiscountType);
    public IEnumerable<object> GetData()
    {
        return VoucherDiscountType.List();
    }
}
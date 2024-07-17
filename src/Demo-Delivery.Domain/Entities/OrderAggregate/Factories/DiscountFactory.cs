using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Domain.Exceptions;

namespace Demo_Delivery.Domain.Entities.OrderAggregate.Factories;

public static class DiscountFactory
{
    public static Func<decimal, decimal> GetDiscountStrategy(Voucher voucher)
    {
        if (voucher.VoucherDiscountType.Id == VoucherDiscountType.Percentage.Id)
        {
            return basePrice => basePrice * voucher.DiscountValue / 100;
        }
        else if (voucher.VoucherDiscountType.Id == VoucherDiscountType.Value.Id)
        {
            return basePrice => voucher.DiscountValue;
        }
        else
        {
            throw new VoucheringDomainException("Invalid value for discount type. " + VoucherDiscountType.ValuesToString());
        }
    }
}
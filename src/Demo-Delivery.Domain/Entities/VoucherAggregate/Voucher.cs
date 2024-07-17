using System.ComponentModel.DataAnnotations.Schema;
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.VoucherAggregate;

public class Voucher : Entity, IAggregateRoot
{
    public string Code { get; private set; }
    public decimal DiscountValue { get; private set; }
    public int Quantity { get; private set; }
    public VoucherDiscountType VoucherDiscountType { get; private set; }
    private int _voucherDiscountTypeId;
    public DateTime? AvailableFrom { get; private set; }
    public DateTime AvailableTo { get; private set; }
    
    [NotMapped]
    public bool IsActive => AvailableTo <= DateTime.UtcNow && AvailableFrom >= DateTime.UtcNow && Quantity > 0;
    public bool WasUsed { get; private set; }

    protected Voucher()
    {
    }

    public Voucher(string code, decimal discountValue, int quantity, VoucherDiscountType discountType,
        DateTime availableTo, DateTime availableFrom)
    {
        AvailableTo = availableTo;
        AvailableFrom = availableFrom;
        Code = code;
        DiscountValue = discountValue;
        Quantity = quantity;
        VoucherDiscountType = discountType;
    }

    public void SetAsUsed()
    {
        WasUsed = true;
        Quantity = 0;
    }

    public void UseUnit()
    {
        Quantity -= 1;

        if (Quantity <= 0)
        {
            SetAsUsed();
        }
    }

     
}
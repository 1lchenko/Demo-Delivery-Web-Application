using Demo_Delivery.Domain.Entities.OrderAggregate.Factories;
using Demo_Delivery.Domain.Entities.VoucherAggregate;
using Demo_Delivery.Domain.Events;
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;
using static Demo_Delivery.Domain.Entities.OrderAggregate.OrderStatus;

namespace Demo_Delivery.Domain.Entities.OrderAggregate;

public class Order : AuditableEntity, IAggregateRoot
{
    public int CustomerId { get; private set; }
    public int? VoucherId { get; private set; }
    public decimal BasePrice => _orderItems.Sum(x => x.TotalPrice);
    public decimal FinalPrice => BasePrice - Discount < 0 ? 0 : BasePrice - Discount;
    public string? Comment { get; private set; }
    public DateTime? WhenDeliver { get; private set; }
    public bool DeliverForNearFuture { get; private set; }
    public decimal Discount { get; private set; }
    public bool HasVoucher { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    private int _orderStatusId;
    public Address Address { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    private readonly List<OrderItem> _orderItems = new();

    private Func<decimal, decimal> _discountStrategy;

    public Order(int customerId, string? comment, DateTime? whenDeliver, bool deliverForNearFuture, Address address)
    {
        CustomerId = customerId;
        Comment = comment;
        Address = address;
        _orderStatusId = Submitted.Id;
        DeliverForNearFuture = deliverForNearFuture;
        if (whenDeliver.HasValue) WhenDeliver = whenDeliver.Value.ToUniversalTime();
        AddOrderStartedDomainEvent(customerId);
    }

    protected Order()
    {
    }

    public void UpdateOrder(string status, DateTime? whenDeliver, bool deliverForNearFuture, Address address)
    {
        Address = address;
        UpdateOrderStatus(status);
        DeliverForNearFuture = deliverForNearFuture;
        WhenDeliver = whenDeliver?.ToUniversalTime();
    }

    private void UpdateOrderStatus(string status)
    {
        var orderStatus = Enumeration.FromDisplayName<OrderStatus>(status);

        switch (orderStatus.Id)
        {
            case var id when id == _orderStatusId && id != Submitted.Id:
                break;
            case var id when id == Submitted.Id:
                throw new InvalidOperationException(
                    "Order is already submitted. You can't change the status to submitted again.");
            case var id when id == AwaitingValidation.Id:
                SetAwaitingValidationStatus();
                break;
            case var id when id == StockConfirmed.Id:
                SetStockConfirmedStatus();
                break;
            case var id when id == Paid.Id:
                SetPaidStatus();
                break;
            case var id when id == Shipped.Id:
                SetShippedStatus();
                break;
            case var id when id == Cancelled.Id:
                SetCancelledStatus();
                break;
            default:
                throw new InvalidOperationException("Invalid order status");
        }
    }

    

    public void SetVoucher(Voucher voucher)
    {
        VoucherId = voucher.Id;
        HasVoucher = true;
        ConfigureDiscountStrategy(voucher);
        CalculateOrderPriceWithDiscount();

        AddDomainEvent(new VoucherAppliyedToOrderEvent(voucher));
    }

    public void AddOrderItem(int productId, string productName, decimal unitPrice, string? productImageKey,
        int quantity = 1)
    {
        var dishItemExist = _orderItems.FirstOrDefault(x => x.ProductId == productId);

        if (dishItemExist != null)
        {
            dishItemExist.AddQuantity(quantity);
        }
        else
        {
            var orderItem = new OrderItem(productId, productName, quantity, unitPrice, productImageKey);
            _orderItems.Add(orderItem);
        }
    }

    public void SetAwaitingValidationStatus()
    {
        if (_orderStatusId != Submitted.Id) StatusChangeException(AwaitingValidation);

        _orderStatusId = AwaitingValidation.Id;
    }

    public void SetStockConfirmedStatus()
    {
        if (_orderStatusId != AwaitingValidation.Id) StatusChangeException(StockConfirmed);

        _orderStatusId = StockConfirmed.Id;
    }

    public void SetPaidStatus()
    {
        if (_orderStatusId != StockConfirmed.Id) StatusChangeException(Paid);

        _orderStatusId = Paid.Id;
    }

    public void SetShippedStatus()
    {
        if (_orderStatusId != Paid.Id) StatusChangeException(Shipped);

        _orderStatusId = Shipped.Id;
    }

    public void SetCancelledStatus()
    {
        if (_orderStatusId == Paid.Id || _orderStatusId == Shipped.Id) StatusChangeException(Cancelled);

        _orderStatusId = Cancelled.Id;
    }

    private void ConfigureDiscountStrategy(Voucher voucher)
    {
        _discountStrategy = DiscountFactory.GetDiscountStrategy(voucher);
    }

    private void CalculateOrderPriceWithDiscount()
    {
        Discount = _discountStrategy.Invoke(BasePrice);
    }

    private void StatusChangeException(OrderStatus orderStatusToChange)
    {
        var orderStatus = FromId(_orderStatusId);

        throw new OrderingDomainException(
            $"Is not possible to change the order status from {orderStatus.Name} to {orderStatusToChange}.");
    }
    

    private void AddOrderStartedDomainEvent(int customerId)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(this, customerId);

        AddDomainEvent(orderStartedDomainEvent);
    }
}
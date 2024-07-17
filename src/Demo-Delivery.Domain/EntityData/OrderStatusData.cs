using Demo_Delivery.Domain.Entities.OrderAggregate;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.EntityData;

public class OrderStatusData : IInicialData
{
    public Type EntityType => typeof(OrderStatus);
    public IEnumerable<object> GetData()
    {
        return OrderStatus.List();
    }
}
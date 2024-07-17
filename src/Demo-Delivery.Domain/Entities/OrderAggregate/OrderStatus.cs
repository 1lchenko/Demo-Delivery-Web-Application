using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.OrderAggregate;

public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Submitted = new OrderStatus(1, nameof(Submitted));
    public static readonly OrderStatus AwaitingValidation = new OrderStatus(2, "Awaiting Validation");
    public static readonly OrderStatus StockConfirmed = new OrderStatus(3, "Stock Confirmed");
    public static readonly OrderStatus Paid = new OrderStatus(4, nameof(Paid));
    public static readonly OrderStatus Shipped = new OrderStatus(5, nameof(Shipped));
    public static readonly OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled));
    
    public OrderStatus(int id, string name) : base(id, name)
    {
    }

    public static IEnumerable<OrderStatus> List() => new[]
    {
        Submitted, AwaitingValidation, StockConfirmed, Paid, Shipped, Cancelled
    };

    public static OrderStatus FromName(string name)
    {
        var status = List().FirstOrDefault(x => String.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (status == null)
        {
            throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
        }

        return status;
    }
    
    public static OrderStatus FromId(int id)
    {
        var status = List().FirstOrDefault(x => x.Id == id);
        if (status == null)
        {
            throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
        }

        return status;
    }
    
}
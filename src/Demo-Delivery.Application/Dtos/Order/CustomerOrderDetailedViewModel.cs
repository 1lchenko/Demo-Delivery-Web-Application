namespace Demo_Delivery.Application.Dtos.Order;

public class CustomerOrderDetailedViewModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public AddressDto Address { get; set; }
    
    public decimal FinalPrice => BasePrice - Discount < 0 ? 0 : BasePrice - Discount;
    public decimal BasePrice => OrderItems.Sum(x => x.TotalPrice);
    public decimal Discount { get; set; }
    public string? Comment { get; set; }

    public DateTime? WhenDeliver { get; set; }

    public string Status { get; set; }

    public DateTime CreateOn { get; set; }

    public List<OrderItemDetailedDto> OrderItems { get; set; }
}
namespace Demo_Delivery.Application.Dtos.Order;

public class OrderDetailedViewModel
{
    public string Id { get; set; }

    public AddressDto AddressDto { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal BasePrice { get; set; }
    public decimal Discount { get; set; }
    public string? Comment { get; set; }

    public DateTime? WhenDeliver { get; set; }

    public string Status { get; set; }

    public DateTime CreateOrderOn { get; set; }

    public List<OrderItemDetailedDto> OrderItems { get; set; }
}
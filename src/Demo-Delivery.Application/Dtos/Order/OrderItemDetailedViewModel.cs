namespace Demo_Delivery.Application.Dtos.Order;

public class OrderItemDetailedDto
{
    public string ProductName { get; set; }
    public string? ImageKey { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
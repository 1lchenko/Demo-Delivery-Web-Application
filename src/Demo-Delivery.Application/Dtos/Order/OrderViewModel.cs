namespace Demo_Delivery.Application.Dtos.Order;

public class OrderViewModel
{
    public string Id { get; set; }
    
    public DateTime CreateOn { get; set; }
    public decimal FinalSum { get; set; }
    public decimal BaseSum { get; set; }
    public decimal Discount { get; set; }
    public string Status { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }
}
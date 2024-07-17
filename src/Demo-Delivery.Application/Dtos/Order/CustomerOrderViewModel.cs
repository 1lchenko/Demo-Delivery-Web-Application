namespace Demo_Delivery.Application.Dtos.Order;

public class CustomerOrderViewModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? WhenDeliver { get; set; }
    public bool DeliverForNearFuture { get; set; }
    public decimal Discount { get; set; }
    public bool HasVoucher { get; set; }
    public string Status { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}
namespace Demo_Delivery.Application.Dtos.Order;

public class CustomerOrderForUpdateDto
{
    public int Id { get; set; }
    public DateTime? WhenDeliver { get; set; }
    public bool DeliverForNearFuture { get; set; }
    public string Status { get; set; }
    public string? Comment { get; set; }
    public AddressDto Address { get; set; }
}
namespace Demo_Delivery.Application.Dtos.Cart;

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string? ImageKey { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Quantity * Price;
}
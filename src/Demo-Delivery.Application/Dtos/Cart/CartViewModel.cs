namespace Demo_Delivery.Application.Dtos.Cart;

public class CartViewModel
{
    public int CustomerId { get; set; }
    public int CartQuantity => CartItems.Sum(x => x.Quantity);
    public List<CartItemViewModel> CartItems { get; set; }
}
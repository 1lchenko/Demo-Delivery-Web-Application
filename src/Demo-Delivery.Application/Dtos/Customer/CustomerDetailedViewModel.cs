using Demo_Delivery.Application.Dtos.Order;

namespace Demo_Delivery.Application.Dtos.Customer;

public class CustomerDetailedViewModel
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public DateTime? LastPurchaseDate { get; set; }

    public DateTime? LastUpdateCartDate { get; set; }

    public string AdminComment { get; set; }
}
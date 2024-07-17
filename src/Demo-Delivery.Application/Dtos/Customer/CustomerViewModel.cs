

namespace Demo_Delivery.Application.Dtos.Customer;

public class CustomerViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Email { get; set; }

    public DateTime? LastPurchaseDate { get; set; }

    public DateTime? LastUpdateCartDate { get; set; }

    public string AdminComment { get; set; }
}
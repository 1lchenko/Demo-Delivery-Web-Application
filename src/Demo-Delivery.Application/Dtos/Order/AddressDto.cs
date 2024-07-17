namespace Demo_Delivery.Application.Dtos.Order;

public class AddressDto
{
    public string Street { get; set; }

    public string? IntercomPinCode { get; set; }
    public string BuildingNumber { get; set; }
    public int ApartmentNumber { get; set; }

    public string? Note { get; set; }
}
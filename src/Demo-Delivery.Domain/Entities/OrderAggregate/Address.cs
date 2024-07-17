using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.OrderAggregate;

public class Address : Entity
{
    public Address(string street, string? intercomPinCode, string buildingNumber, int apartmentNumber, string? note)
    {
        Street = street;
        IntercomPinCode = intercomPinCode;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        Note = note;
    }

    public string Street { get; private set; }

    public string? IntercomPinCode { get; private set; }
    public string BuildingNumber { get; private set; }
    public int ApartmentNumber { get; private set; }

    public string? Note { get; private set; }
}

using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.CustomerAggregate;

public class Customer : Entity, IAggregateRoot
{
    public string Name { get; private set; }

    public string Email { get; private set; }
    public string UserId { get; private set; }

    public DateTime? LastPurchaseDateUtc { get; private set; }

    public DateTime? LastUpdateCartDateUtc { get; private set; }

    public string? AdminComment { get; private set; }

    public void Update(string name, string email, string adminComment)
    {
        Name = name;
        Email = email;
        AdminComment = adminComment;
    }
     
    public Customer(string name, string userId, string email)
    {
        this.Name = name;
        this.UserId = userId;
        this.Email = email;
    }
}
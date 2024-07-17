using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.CategoryAggregate;

public class Category : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }


    protected Category()
    {
    }

    public Category(string name)
    {
     
        Name = name;
    }
    
    public Category UpdateName(string name)
    {
        Name = name;
        return this;
    }
    
}
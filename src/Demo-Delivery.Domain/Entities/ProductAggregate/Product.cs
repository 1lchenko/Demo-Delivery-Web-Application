using Demo_Delivery.Domain.Entities.CategoryAggregate;
using Demo_Delivery.Domain.Exceptions;
using Demo_Delivery.Domain.SeedWork;

namespace Demo_Delivery.Domain.Entities.ProductAggregate;

public class Product : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public Category Category { get; private set; }
    public decimal Calories { get; private set; }
    public int AmountOnStock { get; private set; }
    public int MaxStockThreshold { get; private set; }
    public bool IsActive { get; private set; }
    private readonly List<string> _imageKeys = new List<string>();
    public List<string> ImageKeys => _imageKeys.ToList();

    protected Product()
    {
         
    }

    public Product(string name, string description, decimal price, decimal calories, int amountOnStock,
        int maxStockThreshold, bool isActive, Category category, List<string>? imageKeys = null)
    {
        
        AddStock(amountOnStock);
        MaxStockThreshold = maxStockThreshold;
        AmountOnStock = amountOnStock;
        Name = name;
        Description = description;
        Price = price;
        Calories = calories;
        Category = category;
        IsActive = isActive;
        AddImageKeys(imageKeys);
    }
    
    
    public void Update(string name, string description, decimal price, decimal calories, int amountOnStock,
        int maxStockThreshold, bool isActive, Category category)
    {
        this.UpdateName(name)
            .UpdateDescription(description)
            .UpdatePrice(price)
            .UpdateProductCategory(category)
            .UpdateCalories(calories)
            .UpdateMaxStockThreshold(maxStockThreshold)
            .UpdateIsActive(isActive)
            .AddStock(amountOnStock);
        
    }

    public Product UpdateName(string name)
    {
        Name = name;
        return this;
    }

    public Product UpdateIsActive(bool isActive)
    {
        IsActive = isActive;
        return this;
    }

    public Product UpdateDescription(string description)
    {
        Description = description;
        return this;
    }

    public Product UpdateMaxStockThreshold(int maxStockThreshold)
    {
        MaxStockThreshold = maxStockThreshold;
        return this;
    }

    public Product UpdateProductCategory(Category categoryToUpdate)
    {
        Category = categoryToUpdate;
        return this;
    }

    public Product UpdatePrice(decimal price)
    {
        Price = price;
        return this;
    }

    public Product UpdateCalories(decimal calories)
    {
        Calories = calories;
        return this;
    }

    public void AddImageKeys(List<string>? imageKeys)
    {
        if (imageKeys is null)
        {
            return;
        }
        
        foreach (var imageKey in imageKeys)
        {
            AddImageKey(imageKey);
        }
    }

    public void AddImageKey(string imageKey)
    {
        if (_imageKeys.Count >= 3)
        {
            throw new CatalogDomainException("Cannot add more than three images to a product.");
        }
        
        var existImageKey = _imageKeys.Any(x => x == imageKey);
        if (!existImageKey)
        {
            _imageKeys.Add(imageKey);
        }
    }

    public void RemoveImageKey(string imageKey)
    {
        var existImageKey = _imageKeys.Any(x => x == imageKey);
        if (existImageKey)
        {
            _imageKeys.Remove(imageKey);
        }
    }

    public int RemoveStock(int quantityDesired)
    {
        if (AmountOnStock == 0)
        {
            throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");
        }
        int removed = Math.Min(quantityDesired, AmountOnStock);
        AmountOnStock -= removed;
        return removed;
    }

    public Product AddStock(int quantity)
    {
        int original = AmountOnStock;
        if ((AmountOnStock + quantity) > MaxStockThreshold)
        {
            AmountOnStock = Math.Max(quantity, MaxStockThreshold);
        }
        else
        {
            AmountOnStock += quantity;
        }

        return this;
    }
}
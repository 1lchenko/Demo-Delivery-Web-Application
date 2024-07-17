using Ardalis.Specification;
using Demo_Delivery.Application.Dtos.Product;

namespace Demo_Delivery.Application.Specifications.Product;

public class AdminProductDetailsByIdSpecification : Specification<Domain.Entities.ProductAggregate.Product, AdminProductDetailsDto>
{
    public AdminProductDetailsByIdSpecification(int id)
    {
        Query.Select(x => new AdminProductDetailsDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            AmountOnStock = x.AmountOnStock,
            MaxStockThreshold = x.MaxStockThreshold,
            Calories = x.Calories,
            CategoryName = x.Category.Name,
            CategoryId = x.Category.Id,
            ImagesKeys = x.ImageKeys,
            Price = x.Price
        }).Where(x => x.Id == id);
    }
}
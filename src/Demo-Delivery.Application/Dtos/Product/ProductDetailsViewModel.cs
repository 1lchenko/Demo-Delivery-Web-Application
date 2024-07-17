namespace Demo_Delivery.Application.Dtos.Product;

public class ProductDetailedViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
    public int CategoryId { get; set; }
    public decimal Calories { get; set; }
    public int AmountOnStock { get; set; }

    public bool IsOnStock => AmountOnStock > 0;
    public IEnumerable<string> ImagesKeys { get; set; }

    public List<ProductViewModel> SimilarProducts { get; set; }
}
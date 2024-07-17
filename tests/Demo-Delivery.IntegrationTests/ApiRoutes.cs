namespace Demo_Delivery.IntegrationTests;

public class ApiRoutes
{
    private const string BaseApiUrl = "api";
    private const string BaseAdminApiUrl = "api/admin";

    public static class Catalog
    {
        public const string CreateProduct = $"{BaseAdminApiUrl}/product";
        public const string GetAllProducts = $"{BaseApiUrl}/product";
        public const string GetProductById = $"{BaseApiUrl}/product";
    }
    
    public static class Order
    {
        public const string CreateOrder = $"{BaseApiUrl}/order";
    }
}
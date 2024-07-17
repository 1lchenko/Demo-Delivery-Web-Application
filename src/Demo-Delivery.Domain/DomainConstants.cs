namespace Demo_Delivery.Domain;

public static class DomainConstants
{
    public static class CategoryValidationConstants
    {
        public const int MinNameLength = 0;
        public const int MaxNameLength = 10;
        
        public const int MaxCommentLength = 200;
    }
    public static class ProductValidationConstants
    {
        public const int MinNameLength = 0;
        public const int MaxNameLength = 15;
        
        public const decimal MinPriceValue = 0.01m;
        public const decimal MaxPriceValue = 10000;
        
        public const int MinDescriptionLength = 31;
        public const int MaxDescriptionLength= 68;
        
        public const decimal MinCaloriesValue = 0.01m;
        public const decimal MaxCaloriesValue = 10000;

        public const int MinAmountOnStockValue = 0;
        public const int MaxAmountOnStockValue = 10000;
         
        public const int StockThresholdLowerLimit = 1;
        public const int StockThresholdUpperLimit = 10000;
        
        public const int MaxFileLength = 100 * 1024 * 1024;
       
    }

    public static class OrderValidationConstants
    {
        public const int MaxVoucherCodeLength = 20;
        public const int MaxCommentLength = 500;
    }

    public static class AddressValidationConstants
    {
        public const int MinStreetLength = 1;
        public const int MaxStreetLength = 100;

        public const int MinIntercomPinCodeLength = 0;
        public const int MaxIntercomPinCodeLength = 10;

        public const int MinBuildingNumberLength = 1;
        public const int MaxBuildingNumberLength = 50;

        public const int MinApartmentNumber = 1;
        public const int MaxApartmentNumber = 999; 

        public const int MinNoteLength = 0;
        public const int MaxNoteLength = 200;
    }

    public static class CartItemValidationConstants
    {
        public const int MaxQuantity = 1;
        public const int MinQuantity = 100;
    }
}
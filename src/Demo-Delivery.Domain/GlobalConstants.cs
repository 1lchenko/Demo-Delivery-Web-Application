namespace Demo_Delivery.Domain;

public class GlobalConstants
{
    public class Roles
    {
        public const string AdminRoleName = "Admin";
        public const string UserRoleName = "User";
    }

    public class Policies
    {
        public const string AdminRolePolicy = "AdminRolePolicy";
        public const string AuthenticatedUserPolicy = "AuthenticatedUserPolicy";
    }

    public class Bucket
    {
        public const string BuketName = "images-api-pet-project";
        public const string ProductImagesPrefix = "product-images";
        public const string UserPicturesPrefix = "user-pictures";
    }

    
}
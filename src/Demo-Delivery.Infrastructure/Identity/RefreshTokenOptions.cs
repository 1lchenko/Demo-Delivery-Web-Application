namespace Demo_Delivery.Infrastructure.Identity;

public class RefreshTokenOptions
{
    public const string RefreshToken = "RefreshToken";

    public double RefreshTokenTTL { get; set; }
}
using Microsoft.IdentityModel.Tokens;

namespace Demo_Delivery.Application.Options;

public class JWTOptions
{
    public const string JWT = "JWTOptions";

    public string Issuer { get; set; }
    public string Secret { get; set; }
    public string Audience { get; set; }

    public DateTime Expiration => IssuedAt.Add(ValidFor);

    public DateTime NotBefore => DateTime.UtcNow;

    public DateTime IssuedAt => DateTime.UtcNow;

    public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(15);

    public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());

    public SigningCredentials SigningCredentials { get; set; }
}
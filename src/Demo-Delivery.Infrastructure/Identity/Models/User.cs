using Microsoft.AspNetCore.Identity;

namespace Demo_Delivery.Infrastructure.Identity.Models;

public class User : IdentityUser
{
    public string? ProfilePictureKey { get; set; }
    public DateTime LastPasswordReset { get; set; }
    public DateTime VerifiedEmailTime { get; set; }

    public DateTime CreateOn { get; set; }
    
    private bool _emailConfirmed;
    public override bool EmailConfirmed
    {
        get => _emailConfirmed;
        set
        {
            _emailConfirmed = value;
            VerifiedEmailTime = DateTime.UtcNow;
        }
    }

    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public RefreshToken GetRefreshToken(string value) 
        => RefreshTokens.FirstOrDefault(token => token.Value == value);
    
    public bool OwnsToken(string token)
    {
        return RefreshTokens.FirstOrDefault(x => x.Value == token) != null;
    }
}
namespace Demo_Delivery.Infrastructure.Identity.Models;

public class RefreshToken 
{
    public string Value { get; set; }
    public DateTime ExpiresOn { get; set; } 
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string CreatedByIp { get; set; }
    public DateTime? RevokeOn { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByRefreshToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpires => DateTime.UtcNow >= ExpiresOn;
    public bool IsRevoked => RevokeOn.HasValue;
    public bool IsActive => !RevokeOn.HasValue && !IsExpires;
}
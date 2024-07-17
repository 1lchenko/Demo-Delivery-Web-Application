using System.Security.Claims;
using Demo_Delivery.Infrastructure.Identity.Models;

namespace Demo_Delivery.Infrastructure.Services;

public interface IJwtService
{
    public Task<JwtToken> GenerateJwtToken(ClaimsIdentity claimsIdentity);
    public RefreshToken GenerateRefreshTokenAsync(string ipAddress);
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Demo_Delivery.Application.Options;
using Demo_Delivery.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Demo_Delivery.Infrastructure.Services;

public class JwtService : IJwtService
{
   
    private readonly JWTOptions _jwtOptions;

    public JwtService(IOptions<JWTOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<JwtToken> GenerateJwtToken(ClaimsIdentity claimsIdentity)
    {
        claimsIdentity.AddClaims(new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
            new Claim(JwtRegisteredClaimNames.Iat, _jwtOptions.IssuedAt.ToString())  
        });

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = claimsIdentity,
            NotBefore = _jwtOptions.NotBefore,
            Expires = _jwtOptions.Expiration,
            SigningCredentials = _jwtOptions.SigningCredentials,
        });

        return new JwtToken { Id = token.Id, Value = tokenHandler.WriteToken(token) };
    }
    
    public RefreshToken GenerateRefreshTokenAsync(string ipAddress)
    {
        var refreshToken = new RefreshToken
        {
            Value = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresOn = DateTime.UtcNow.AddDays(1),
            CreatedOn = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };

        return refreshToken;
    }
}
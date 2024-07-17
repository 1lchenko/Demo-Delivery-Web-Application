using System.Security.Claims;
using System.Text.Encodings.Web;
using Demo_Delivery.Domain;
using Demo_Delivery.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Demo_Delivery.Infrastructure.TestBase;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";

    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
         
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, TestUserSettings.UserId),
                new Claim(ClaimTypes.Name, TestUserSettings.Name),
                new Claim(ClaimTypes.Email, TestUserSettings.UserEmail),
                new Claim(ClaimTypes.Role, GlobalConstants.Roles.AdminRoleName),
            };

        
            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        

         
    }
}
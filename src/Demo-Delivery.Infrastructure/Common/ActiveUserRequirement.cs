using Demo_Delivery.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Demo_Delivery.Infrastructure.Common;

public class ActiveUserRequirement : AuthorizationHandler<ActiveUserRequirement>, IAuthorizationRequirement
{
    private readonly IServiceProvider _serviceProvider;

    public ActiveUserRequirement(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated is false)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();
        var user = await signInManager.ValidateSecurityStampAsync(context.User);
        if (user is null)
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }
    }
}
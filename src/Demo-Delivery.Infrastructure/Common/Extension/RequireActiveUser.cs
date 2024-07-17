using Microsoft.AspNetCore.Authorization;

namespace Demo_Delivery.Infrastructure.Common.Extension;

public static class RequireActiveUserExtension
{
    public static AuthorizationPolicyBuilder RequireActiveUser(this AuthorizationPolicyBuilder builder, IServiceProvider provider)
    {
        builder.Requirements.Add(new ActiveUserRequirement(provider));
        return builder;
    }
}
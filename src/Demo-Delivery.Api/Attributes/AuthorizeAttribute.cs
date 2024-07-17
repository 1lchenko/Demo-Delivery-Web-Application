/*using Demo_Delivery.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo_Delivery.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<string> _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles ?? new string[] { };
    }

    public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
    {
        var allowAnonymous =
            authorizationFilterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous) return;

        var account = authorizationFilterContext.HttpContext.Items["User"] as User;

        if ((_roles.Count > 0 && !_roles.Contains(account?.Role.Name)) || account == null)
            authorizationFilterContext.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
    }
}*/
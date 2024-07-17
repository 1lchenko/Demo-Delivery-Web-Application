using System.Security.Claims;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Demo_Delivery.Infrastructure.Identity;

public class AspNetUser : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Id => _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string UserName => _accessor.HttpContext.User.Identity.Name;

    public bool HasRole(string roleName)
    {
        return _accessor.HttpContext.User.IsInRole(roleName);
    }

    public bool IsAuthenticated() => _accessor.HttpContext.User.Identity.IsAuthenticated;

    public IEnumerable<Claim> GetClaimsIdentity() => _accessor.HttpContext.User.Claims;
}
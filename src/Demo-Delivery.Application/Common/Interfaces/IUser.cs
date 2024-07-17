using System.Security.Claims;

namespace Demo_Delivery.Application.Common.Interfaces;

public interface IUser
{
    public string Id { get; }
    public string UserName { get; }
    
    public bool IsAuthenticated();

    public bool HasRole(string roleName);

    public IEnumerable<Claim> GetClaimsIdentity();
}
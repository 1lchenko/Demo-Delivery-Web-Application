using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo_Delivery.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    public async Task<List<string>> GetAllAsync()
    {
        return (await _roleManager.Roles.Select(r => r.Name).ToListAsync())!;
    }
}
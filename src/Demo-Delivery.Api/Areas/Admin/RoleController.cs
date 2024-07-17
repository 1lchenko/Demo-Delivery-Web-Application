using Demo_Delivery.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Areas.Admin;

public class RoleController : BaseAdminController
{
    // may be we can use RoleManager<IdentityRole> instead of IRoleService ?? // when you will watch the video you will understand
    private readonly IRoleService _roleService;
    
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _roleService.GetAllAsync());
    }
}
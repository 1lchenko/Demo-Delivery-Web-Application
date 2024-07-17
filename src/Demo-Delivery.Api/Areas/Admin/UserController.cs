using Demo_Delivery.Api.Extensions;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Account;
using Demo_Delivery.Application.Dtos.Account.Requests;
using Demo_Delivery.Application.Dtos.Identity.Requests;
using Demo_Delivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Delivery.Api.Areas.Admin;

public class UserController : BaseAdminController
{
    private readonly IUser _currentUser;
    private readonly IUserService _userService;

    public UserController(IUserService userService, IUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersRequestModel requestModel)
    {
        var users = await _userService.GetAllAsync(requestModel);
        HttpContext.Response.AddPagination(users.CurrentPage, users.TotalCount, users.TotalPages, users.HasNextPage, users.HasPreviousPage);
        return Ok(users);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound($"User({id}) is not found");
        }

        return Ok(user);
    }

    [HttpGet]
    [Route("getUserByIdForUpdate/{id}")]
    public async Task<IActionResult> GetUserByIdForUpdate(string id) =>
        Ok(await _userService.GetUserByIdForUpdateAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequestModel model)
    {
        await _userService.CreateAsync(model, Request.Headers["origin"]);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserRequestDto model)
    {
        if (_currentUser.Id != model.Id && !_currentUser.HasRole(GlobalConstants.Roles.AdminRoleName))
            return Unauthorized("Unauthorized");
        var updatedAccount = await _userService.UpdateAsync(model);
        return Ok(updatedAccount);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        if (_currentUser.Id == id) throw new BadRequestException("Cannot delete your account");
        await _userService.DeleteAsync(id);
        return Ok();
    }
}
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Account;
using Demo_Delivery.Application.Dtos.Account.Requests;
using Demo_Delivery.Application.Dtos.Identity.Requests;
using Demo_Delivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.RequestModels;

namespace Demo_Delivery.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = GlobalConstants.Policies.AuthenticatedUserPolicy)]
public class AccountController : ControllerBase
{
    private readonly IUser _currentUser;
    private readonly IUserService _userService;

    public AccountController(IUserService userService, IUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    [HttpGet]
    [Route(nameof(GetCurrentUser))]
    public async Task<IActionResult> GetCurrentUser() => Ok(await _userService.GetCurrentUserAsync(_currentUser.Id));

    [HttpPost]
    [Route(nameof(Login))]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequestModel model)
    {
        var userViewModel = await _userService.LoginAsync(model, GetIpAddress());
        SetRefreshTokenInCookie(userViewModel.RefreshToken);
        return Ok(userViewModel);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route(nameof(LogOut))]
    public async Task<IActionResult> LogOut()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized();

        await _userService.LogOutAsync(_currentUser.Id, refreshToken, GetIpAddress());

        return Ok();
    }

    [HttpPost]
    [Route(nameof(Register))]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequestModel model)
    {
        await _userService.RegisterAsync(model, Request.Headers["origin"]);
        return Ok();
    }

    [HttpPost]
    [Route(nameof(RefreshToken))]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var userViewModel = await _userService.RefreshTokenAsync(refreshToken, GetIpAddress()!);
        SetRefreshTokenInCookie(userViewModel.RefreshToken);
        return Ok(userViewModel);
    }

    [HttpPost]
    [Route(nameof(ChangePassword))]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel model)
    {
        await _userService.ChangePasswordAsync(model, _currentUser.Id);
        return Ok();
    }
    
    [HttpPut]
    [Route(nameof(UpdateUserProfilePicture))]
    public async Task<IActionResult> UpdateUserProfilePicture([FromForm]
        UpdateUserProfilePictureKeyModel profilePictureKeyModel)
    {
        if (_currentUser.Id != profilePictureKeyModel.UserId)
        {
            return BadRequest("Invalid value of current UserId");
        }

        await _userService.UpdateUserPictureAsync(profilePictureKeyModel);
        return Ok();
    }

    [HttpPost]
    [Route(nameof(ValidateConfirmEmailToken))]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateConfirmEmailToken(ValidateConfirmEmailTokenRequestModel model)
    {
        await _userService.ValidateConfirmEmailTokenAsync(model.EmailVerificationToken, model.Email);
        return Ok();
    }
    
    [HttpPost]
    [Route(nameof(VerifyEmail))]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail(VerifyEmailRequestModel model)
    {
        await _userService.VerifyEmailAsync(model.Email, Request.Headers["origin"]);
        return Ok();
    }

    [HttpPost]
    [Route(nameof(ForgotPassword))]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestModel model)
    {
        await _userService.ForgotPasswordAsync(model, Request.Headers["origin"]);
        return Ok();
    }

    [HttpPost]
    [Route(nameof(ValidateResetToken))]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateResetToken(ValidateResetTokenRequestModel model)
    {
        var result = await _userService.ValidateResetTokenAsync(model.ResetToken, _currentUser.Id);
        return result ? Ok() : BadRequest("Invalid token");
    }

    [HttpPost]
    [Route(nameof(ResetPassword))]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
    {
        await _userService.ResetPasswordAsync(model, _currentUser.Id);
        return Ok();
    }

    private void SetRefreshTokenInCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            Secure = true, HttpOnly = true, SameSite = SameSiteMode.None, Expires = DateTime.UtcNow.AddDays(1)
        };

        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string? GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
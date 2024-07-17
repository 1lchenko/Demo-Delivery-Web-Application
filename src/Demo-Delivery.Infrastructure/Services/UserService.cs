using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Demo_Delivery.Application.Common.Abstractions.Services;
using Demo_Delivery.Application.Common.Exceptions;
using Demo_Delivery.Application.Common.Interfaces;
using Demo_Delivery.Application.Dtos.Account;
using Demo_Delivery.Application.Dtos.Account.Requests;
using Demo_Delivery.Application.Dtos.Identity;
using Demo_Delivery.Application.Dtos.Identity.Requests;
using Demo_Delivery.Application.Dtos.Identity.Responses;
using Demo_Delivery.Application.Filtration;
using Demo_Delivery.Domain;
using Demo_Delivery.Domain.Events;
using Demo_Delivery.Infrastructure.Data;
using Demo_Delivery.Infrastructure.Identity;
using Demo_Delivery.Infrastructure.Identity.Exceptions;
using Demo_Delivery.Infrastructure.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Models.RequestModels;

namespace Demo_Delivery.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly IJwtService _jwtService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;
    private readonly IFileService _fileService;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService,
        ApplicationDbContext dbContext, IEmailService emailService, IOptions<RefreshTokenOptions> refreshTokenOptions,
        IMediator mediator, IFileService fileService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _dbContext = dbContext;
        _emailService = emailService;
        _mediator = mediator;
        _fileService = fileService;

        _refreshTokenOptions = refreshTokenOptions.Value;
    }

    public async Task UpdateUserPictureAsync(UpdateUserProfilePictureKeyModel userProfilePictureKeyModel)
    {
        var user = await _userManager.FindByIdAsync(userProfilePictureKeyModel.UserId);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), userProfilePictureKeyModel.UserId);
        }

        if (user.ProfilePictureKey != null)
        {
            await _fileService.DeleteFileAsync(user.ProfilePictureKey, GlobalConstants.Bucket.BuketName);
        }
        
        var pictureKey = await _fileService.UploadFileAsync(userProfilePictureKeyModel.NewUserPicture,
            GlobalConstants.Bucket.BuketName, GlobalConstants.Bucket.UserPicturesPrefix);

        user.ProfilePictureKey = pictureKey;

        await _userManager.UpdateAsync(user);
    }

    public async Task<CurrentUserResponseModel> GetCurrentUserAsync(string userId)
    {
        var currentUser = await _userManager.Users.Where(u => u.Id == userId)
            .Select(u => new CurrentUserResponseModel()
            {
                Id = u.Id, Email = u.Email, UserName = u.UserName, ProfilePictureKey = u.ProfilePictureKey
            })
            .FirstOrDefaultAsync();

        if (currentUser is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        return currentUser;
    }

    public async Task<Account> LoginAsync(LoginRequestModel loginModel, string ipAddress)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if (user is null) throw new NotFoundException(nameof(User), loginModel.Email);

        var signInResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);

        switch (signInResult.Succeeded)
        {
            case false when !user.EmailConfirmed:
                throw new IdentityOperationException("Email is not confirmed");
            case false:
                throw new IdentityOperationException("Password or email is incorrect");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var jwtToken = await GenerateJwtTokenAsync(user, userRoles);

        var refreshToken = _jwtService.GenerateRefreshTokenAsync(ipAddress);
        await RemoveOldRefreshTokensAsync(user);
        user.RefreshTokens.Add(refreshToken);

        await _dbContext.SaveChangesAsync();

        var viewModel = new Account
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            JwtToken = jwtToken.Value,
            RefreshToken = refreshToken.Value,
            Roles = userRoles.ToList()
        };

        return viewModel;
    }

    public async Task<Account> RefreshTokenAsync(string? token, string ipAddress)
    {
        var user = await GetUserByRefreshTokenAsync(token);

        if (user is null) throw new BadRequestException("Refresh token is incorrect");

        var refreshToken = user.GetRefreshToken(token);

        if (!refreshToken.IsActive) throw new BadRequestException("Refresh Token Is Invalid");

        var newRefreshToken = RotateRefreshTokenAsync(refreshToken, ipAddress);
        user.RefreshTokens.Add(newRefreshToken);
        await RemoveOldRefreshTokensAsync(user);
        await _dbContext.SaveChangesAsync();

        var userRoles = await _userManager.GetRolesAsync(user);
        var jwtToken = await GenerateJwtTokenAsync(user, userRoles);

        var viewModel = new Account
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            JwtToken = jwtToken.Value,
            RefreshToken = newRefreshToken.Value,
            Roles = userRoles.ToList()
        };

        return viewModel;
    }

    public async Task LogOutAsync(string userId, string token, string ipAddress)
    {
        var user = await GetUserByRefreshTokenAsync(token);
        if (user is null || user.Id != userId) throw new BadRequestException("Refresh token is incorrect");

        var refreshToken = user.GetRefreshToken(token);

        if (!refreshToken.IsActive) throw new BadRequestException("Refresh Token Is Obsolete");

        RevokeRefreshToken(refreshToken, ipAddress, "Logout");

        await _dbContext.SaveChangesAsync();
    }

    public async Task RegisterAsync(RegisterRequestModel registerModel, string? origin)
    {
        var user = new User { UserName = registerModel.UserName, Email = registerModel.Email };
        var identityResult = await _userManager.CreateAsync(user, registerModel.Password);

        if (!identityResult.Succeeded) throw new IdentityOperationException(identityResult.Errors);

        identityResult = await _userManager.AddToRoleAsync(user, GlobalConstants.Roles.UserRoleName);

        if (!identityResult.Succeeded) throw new IdentityOperationException(identityResult.Errors);

        var userRegisteredEvent = new UserRegisteredDomainEvent(user.Id, user.UserName, user.Email);
        await _mediator.Publish(userRegisteredEvent);

        var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await SendVerificationEmailAsync(user.Email, verificationToken, origin);
    }

    public async Task VerifyEmailAsync(string email, string origin)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), email);
        }

        if (user.EmailConfirmed)
        {
            throw new BadRequestException("User has already confirmed their email");
        }

        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await SendVerificationEmailAsync(user.Email, emailConfirmationToken, origin);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequestModel model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new BadRequestException("User want to change password but not found in database");
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Errors.Any())
        {
            throw new IdentityOperationException(result.Errors);
        }
    }

    public async Task<bool> UserHasRefreshTokenAsync(string refreshToken, string userId)
    {
        return await _userManager.Users.AnyAsync(u =>
            u.Id == userId && u.RefreshTokens.Any(t => t.Value == refreshToken));
    }

    public async Task<UpdateUserResponseModel> GetUserByIdForUpdateAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        return new UpdateUserResponseModel()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd.HasValue ? user.LockoutEnd.Value.UtcDateTime : null,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }

    public async Task ValidateConfirmEmailTokenAsync(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) throw new NotFoundException(nameof(User), email);
        var identityResult = await _userManager.ConfirmEmailAsync(user, token);
        if (!identityResult.Succeeded) throw new IdentityOperationException(identityResult.Errors);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestModel forgotPasswordModel, string origin)
    {
        var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
        if (user is null)
        {
            throw new BadRequestException(
                "That address is either invalid, or is not associated with a personal user account.");
        }

        if (!user.EmailConfirmed) throw new BadRequestException("Not confirmed email");

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        var bytes = Encoding.UTF8.GetBytes(code);
        code = WebEncoders.Base64UrlEncode(bytes);

        await SendPasswordResetEmail(user.Email, code, origin);
    }

    public async Task<bool> ValidateResetTokenAsync(string resetToken, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var bytes = WebEncoders.Base64UrlDecode(resetToken);
        resetToken = Encoding.UTF8.GetString(bytes);

        var isValid = await _userManager.VerifyUserTokenAsync(user,
            _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);

        return isValid;
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestModel model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new NotFoundException(nameof(User), userId);

        var bytes = WebEncoders.Base64UrlDecode(model.ResetToken);
        var resetToken = Encoding.UTF8.GetString(bytes);

        var identityResult = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

        if (!identityResult.Succeeded) throw new IdentityOperationException(identityResult.Errors);

        user.LastPasswordReset = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PaginatedList<GetAllUsersResponseModel>> GetAllAsync(GetAllUsersRequestModel requestModel)
    {
        var users = _userManager.Users.Join(_dbContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
            .Join(_dbContext.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
            .Where(c => string.IsNullOrEmpty(requestModel.Search) || c.ur.u.UserName.Contains(requestModel.Search) ||
                        c.ur.u.Email.Contains(requestModel.Search))
            .GroupBy(c => new { c.ur.u.Id, c.ur.u.UserName, c.ur.u.Email, c.ur.u.EmailConfirmed })
            .Where(g => requestModel.RoleNames == null || !requestModel.RoleNames.Any() ||
                        g.Any(c => requestModel.RoleNames.Contains(c.r.Name)))
            .Select(g => new GetAllUsersResponseModel()
            {
                Id = g.Key.Id,
                UserName = g.Key.UserName,
                Email = g.Key.Email,
                EmailConfirmed = g.Key.EmailConfirmed,
                RoleNames = string.Join(", ", g.Select(c => c.r.Name)),
            })
            .AsNoTracking();

        return await PaginatedList<GetAllUsersResponseModel>.CreateFromQueryableAsync(users, requestModel.CurrentPage,
            GetAllUsersRequestModel.MaxPageSize);
    }

    public async Task<UserDetailedResponseModel?> GetByIdAsync(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return null;

        var userDetailed = new UserDetailedResponseModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            LastPasswordReset = user.LastPasswordReset,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };

        return userDetailed;
    }

    public async Task CreateAsync(CreateUserRequestModel createUserModel, string origin)
    {
        var user = new User
        {
            UserName = createUserModel.UserName,
            Email = createUserModel.Email,
            EmailConfirmed = createUserModel.EmailConfirmed
        };

        var identityResult = await _userManager.CreateAsync(user, createUserModel.Password);

        if (!identityResult.Succeeded) throw new IdentityOperationException(identityResult.Errors);

        identityResult = await _userManager.AddToRolesAsync(user, createUserModel.RoleNames);

        if (!identityResult.Succeeded) throw new IdentityOperationException(identityResult.Errors);

        if (!user.EmailConfirmed)
        {
            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendVerificationEmailAsync(user.Email, verificationToken, origin);
        }
    }

    public async Task<UpdateUserResponseModel> UpdateAsync(UpdateUserRequestDto model)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), model.Id);
        }

        user.UserName = model.UserName;
        user.Email = model.Email;

        user.LockoutEnd = model.LockoutEnd ?? null;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            throw new IdentityOperationException(updateResult.Errors);
        }

        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword!);
            if (!resetPasswordResult.Succeeded)
            {
                throw new IdentityOperationException(resetPasswordResult.Errors);
            }
        }

        var currentUserRoles = await _userManager.GetRolesAsync(user);

        var rolesToRemove = currentUserRoles.Except(model.Roles);
        var rolesToAdd = model.Roles.Except(currentUserRoles);

        if (rolesToAdd.Any())
        {
            var result = _userManager.AddToRolesAsync(user, model.Roles);
            if (!result.Result.Succeeded)
            {
                throw new IdentityOperationException(result.Result.Errors);
            }
        }

        if (rolesToRemove.Any())
        {
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeRoleResult.Succeeded)
            {
                throw new IdentityOperationException(removeRoleResult.Errors);
            }
        }

        return new UpdateUserResponseModel()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd?.UtcDateTime,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }

    public async Task DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        var identityResult = await _userManager.DeleteAsync(user);
        if (!identityResult.Succeeded)
        {
            throw new IdentityOperationException(identityResult.Errors);
        }
    }

    public async Task AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        var identityResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!identityResult.Succeeded)
        {
            throw new IdentityOperationException(identityResult.Errors);
        }
    }

    public async Task RemoveRoleFromUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        var identityResult = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (!identityResult.Succeeded)
        {
            throw new IdentityOperationException(identityResult.Errors);
        }
    }

    private async Task<JwtToken> GenerateJwtTokenAsync(User user, IList<string> userRoles)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

        var userClaims = await _userManager.GetClaimsAsync(user);
        claimsIdentity.AddClaims(userClaims);

        claimsIdentity.AddClaims(userRoles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

        claimsIdentity.AddClaim(new Claim(new ClaimsIdentityOptions().SecurityStampClaimType, user.SecurityStamp));

        var jwtToken = await _jwtService.GenerateJwtToken(claimsIdentity);

        return jwtToken;
    }

    private async Task<User?> GetUserByRefreshTokenAsync(string? refreshToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(
            x => x.RefreshTokens.Any(token => token.Value == refreshToken));
    }

    private RefreshToken RotateRefreshTokenAsync(RefreshToken refreshToken, string ipAddress)
    {
        var newRefreshToken = _jwtService.GenerateRefreshTokenAsync(ipAddress);
        RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new refreshToken", newRefreshToken.Value);
        return newRefreshToken;
    }

    private async Task RemoveOldRefreshTokensAsync(User account)
    {
        account.RefreshTokens.RemoveAll(x =>
            !x.IsActive && x.CreatedOn.AddDays(_refreshTokenOptions.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    private void RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string? reason = null,
        string? replacedByRefreshToken = null)
    {
        refreshToken.RevokeOn = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByRefreshToken = replacedByRefreshToken;
    }

    private async Task SendVerificationEmailAsync(string email, string verificationToken, string? origin)
    {
        var encodedToken = System.Web.HttpUtility.UrlEncode(verificationToken);
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            var url = $"{origin}/account/verify-email?token={encodedToken}&email={email}";
            message = $@"<p>Please go to the below link for verify your email: </p>
            <p><a href='{url}'>Link</a></p>";
        }
        else
        {
            message = $@"<p>For verify email call this Endpoint (HttpPost): <code>/account/verify-email</code></p>
            <p>With this token: {encodedToken}</p>";
        }

        await _emailService.SendAsync(email, "Verify Email Address", $@"<h1>Thanks for Registration!</h1> {message}");
    }

    private async Task SendPasswordResetEmail(string email, string resetCode, string origin)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            var url = $"{origin}/account/reset-password?token={resetCode}";
            message = $@"<p>Please go to the below link for reset your password: </p>
            <p><a href='{url}'>Link</a></p>   <p>Link active for 24 hours</p>";
        }
        else
        {
            message =
                $@"<p>For reset your password call this Endpoint (HttpPost): <code>/account/reset-password</code></p>
            <p>With this token: {resetCode}</p> <p>Link active for 24 hours</p>";
        }

        await _emailService.SendAsync(email, "Reset Password", message);
    }
}
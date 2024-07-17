using Demo_Delivery.Application.Dtos.Account;
using Demo_Delivery.Application.Dtos.Account.Requests;
using Demo_Delivery.Application.Dtos.Identity;
using Demo_Delivery.Application.Dtos.Identity.Requests;
using Demo_Delivery.Application.Dtos.Identity.Responses;
using Demo_Delivery.Application.Filtration;
using WebApplication1.Models.RequestModels;

namespace Demo_Delivery.Application.Common.Interfaces;

public interface IUserService
{
    Task UpdateUserPictureAsync(UpdateUserProfilePictureKeyModel userProfilePictureKeyModel);
    Task<CurrentUserResponseModel> GetCurrentUserAsync(string userId);
    Task<Account> LoginAsync(LoginRequestModel loginModel, string ipAddress);
    Task<Account> RefreshTokenAsync(string? token, string ipAddress);
    public Task LogOutAsync(string userId, string token, string ipAddress);
    Task RegisterAsync(RegisterRequestModel registerModel, string origin);
    Task ChangePasswordAsync(ChangePasswordRequestModel model, string userId);
    Task<bool> UserHasRefreshTokenAsync(string refreshToken, string userId);
    Task<UpdateUserResponseModel> GetUserByIdForUpdateAsync(string userId);
    Task VerifyEmailAsync(string email, string origin);
    Task ValidateConfirmEmailTokenAsync(string token, string userId);
    Task ForgotPasswordAsync(ForgotPasswordRequestModel forgotPasswordModel, string origin);
    Task<bool> ValidateResetTokenAsync(string resetToken, string userId);
    Task ResetPasswordAsync(ResetPasswordRequestModel model, string userId);
    Task<PaginatedList<GetAllUsersResponseModel>> GetAllAsync(GetAllUsersRequestModel requestModel);
    public Task<UserDetailedResponseModel?> GetByIdAsync(string id);
    Task CreateAsync(CreateUserRequestModel createUserModel, string origin);
    Task<UpdateUserResponseModel> UpdateAsync(UpdateUserRequestDto model);
    Task DeleteAsync(string userId);
    Task AddUserToRoleAsync(string userId, string roleName);
    Task RemoveRoleFromUserAsync(string userId, string roleName);
}
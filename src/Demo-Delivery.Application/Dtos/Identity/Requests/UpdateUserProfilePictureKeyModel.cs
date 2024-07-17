using Microsoft.AspNetCore.Http;

namespace Demo_Delivery.Application.Dtos.Account.Requests;

public class UpdateUserProfilePictureKeyModel
{
    public string UserId { get; set; }
    public IFormFile NewUserPicture { get; set; }
}
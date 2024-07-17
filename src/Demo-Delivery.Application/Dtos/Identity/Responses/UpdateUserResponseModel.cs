namespace Demo_Delivery.Application.Dtos.Account;

public class UpdateUserResponseModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }

    public string UserName { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmNewPassword { get; set; }
    public List<string> Roles { get; set; }
}
namespace Demo_Delivery.Application.Dtos.Account;

public class CreateUserRequestModel
{
    public string Email { get; set; }

    public string UserName { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public List<string> RoleNames { get; set; }
}
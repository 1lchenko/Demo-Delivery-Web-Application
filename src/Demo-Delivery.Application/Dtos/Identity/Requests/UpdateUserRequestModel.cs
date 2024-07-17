using System.ComponentModel.DataAnnotations;

namespace Demo_Delivery.Application.Dtos.Account;

public class UpdateUserRequestDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public DateTime? LockoutEnd { get; set; }

    [Required(ErrorMessage = "Determine Username")]
    public string UserName { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at least {8} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^[a-zA-Z\d&@^!+=—»]{8,100}$",
        ErrorMessage =
            "Password must only contain letters, digits, and the special characters & @ ^ ! + = — » and be between 8 and 100 characters long.")]
    [Compare("ConfirmNewPassword", ErrorMessage = "Passwords do not match")]
    public string? NewPassword { get; set; }

    public string? ConfirmNewPassword { get; set; }
    public List<string> Roles { get; set; }
}
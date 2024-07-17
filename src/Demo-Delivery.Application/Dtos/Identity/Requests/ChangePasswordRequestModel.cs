using System.ComponentModel.DataAnnotations;

namespace Demo_Delivery.Application.Dtos.Account;

public class ChangePasswordRequestModel
{
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; }
    
    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {8} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^[a-zA-Z\d&@^!+=—»]{8,100}$", ErrorMessage = "Password must only contain letters, digits, and the special characters & @ ^ ! + = — » and be between 8 and 100 characters long.")]

    [Compare("ConfirmNewPassword", ErrorMessage = "Passwords do not match")]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Confirmation password is required")] 
    public string ConfirmNewPassword { get; set; }
}
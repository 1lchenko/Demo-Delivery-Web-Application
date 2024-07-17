using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.RequestModels;

public class ResetPasswordRequestModel
{
    [Compare("ConfirmPassword", ErrorMessage = "Passwords do not match")]
    [Required(ErrorMessage = "Determine Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Determine Password")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Determine ResetToken")]
    public string ResetToken { get; set; }
}
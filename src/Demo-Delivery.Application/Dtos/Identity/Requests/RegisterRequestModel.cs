using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.RequestModels;

public class RegisterRequestModel
{
    
    [EmailAddress(ErrorMessage = "Determine valid email address")]
    [Required(ErrorMessage = "Determine Email")]
    public string Email { get; set; }

    [MaxLength(15, ErrorMessage = "Max length = 15")]
    [Required(ErrorMessage = "Determine Username")]
    public string UserName { get; set; }

    [MinLength(6, ErrorMessage = "Min length password must be 6 symbols")]
    [Compare("ConfirmPassword", ErrorMessage = "Passwords do not match")]
    [Required(ErrorMessage = "Determine Password")]
    public string Password { get; set; }


    [Required(ErrorMessage = "Determine ConfirmPassword")]
    public string ConfirmPassword { get; set; }
}
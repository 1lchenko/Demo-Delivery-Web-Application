using System.ComponentModel.DataAnnotations;

namespace Demo_Delivery.Application.Dtos.Identity.Requests;

public class ValidateConfirmEmailTokenRequestModel
{
    [Required(ErrorMessage = "Verify token is required")]
    public string EmailVerificationToken { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email has wrong format")]
    public string Email { get; set; }
}
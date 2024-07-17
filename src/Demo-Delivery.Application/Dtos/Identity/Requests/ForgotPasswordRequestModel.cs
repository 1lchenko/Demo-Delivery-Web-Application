using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.RequestModels;

public class ForgotPasswordRequestModel
{
    [EmailAddress(ErrorMessage = "Determine valid email address")]
    [Required(ErrorMessage = "Determine Email")]
    public string Email { get; set; }
}
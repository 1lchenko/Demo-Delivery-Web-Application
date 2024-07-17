using System.ComponentModel.DataAnnotations;

namespace Demo_Delivery.Application.Dtos.Identity.Requests;

public class LoginRequestModel
{
    [EmailAddress(ErrorMessage = "Determine valid email address")]
    [Required(ErrorMessage = "Determine Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Determine Password")]
    public string Password { get; set; }
}
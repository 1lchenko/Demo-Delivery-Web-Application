using System.ComponentModel.DataAnnotations;

namespace Demo_Delivery.Application.Dtos.Account.Requests;

public class VerifyEmailRequestModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email has wrong format")]
    public string Email { get; set; }
}
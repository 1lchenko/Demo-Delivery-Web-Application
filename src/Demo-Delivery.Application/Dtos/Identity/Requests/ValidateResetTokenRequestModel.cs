using System.ComponentModel.DataAnnotations;

namespace Demo_Delivery.Application.Dtos.Account;

public class ValidateResetTokenRequestModel
{
    [Required(ErrorMessage = "Determine JwtToken")]
    public string ResetToken { get; set; }
    
}